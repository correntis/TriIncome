<template>
  <div class="loan-list">
    <el-card class="filter-card">
      <template #header>
        <div class="card-header">
          <span>Фильтры</span>
          <el-button 
            size="small" 
            @click="resetFilters"
            :icon="RefreshRight"
          >
            Сбросить
          </el-button>
        </div>
      </template>
      
      <el-form :model="filters" label-width="150px">
        <el-row :gutter="20">
          <el-col :span="8">
            <el-form-item label="Статус">
              <el-select 
                v-model="filters.status" 
                placeholder="Все статусы"
                clearable
                @change="applyFilters"
              >
                <el-option label="Все" :value="null" />
                <el-option label="Опубликована" :value="0" />
                <el-option label="Снята с публикации" :value="1" />
              </el-select>
            </el-form-item>
          </el-col>
          
          <el-col :span="8">
            <el-form-item label="Сумма от">
              <el-input-number 
                v-model="filters.minAmount" 
                :min="0"
                :precision="2"
                :step="1000"
                placeholder="Мин. сумма"
                @change="applyFilters"
              />
            </el-form-item>
          </el-col>
          
          <el-col :span="8">
            <el-form-item label="Сумма до">
              <el-input-number 
                v-model="filters.maxAmount" 
                :min="0"
                :precision="2"
                :step="1000"
                placeholder="Макс. сумма"
                @change="applyFilters"
              />
            </el-form-item>
          </el-col>
        </el-row>

        <el-row :gutter="20">
          <el-col :span="8">
            <el-form-item label="Срок займа от">
              <el-input-number 
                v-model="filters.minTerm" 
                :min="0"
                placeholder="Мин. срок"
                @change="applyFilters"
              />
            </el-form-item>
          </el-col>
          
          <el-col :span="8">
            <el-form-item label="Срок займа до">
              <el-input-number 
                v-model="filters.maxTerm" 
                :min="0"
                placeholder="Макс. срок"
                @change="applyFilters"
              />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </el-card>

    <el-card class="table-card" v-loading="loading">
      <template #header>
        <div class="card-header">
          <span>Список заявок ({{ loans.length }})</span>
          <el-button 
            type="primary" 
            @click="$router.push('/create')"
            :icon="Plus"
          >
            Создать заявку
          </el-button>
        </div>
      </template>

      <el-table 
        :data="loans" 
        stripe
        style="width: 100%"
        :default-sort="{ prop: 'createdAt', order: 'descending' }"
      >
        <el-table-column prop="number" label="Номер заявки" width="150" />
        
        <el-table-column prop="amount" label="Сумма" width="150" sortable>
          <template #default="{ row }">
            {{ formatCurrency(row.amount) }}
          </template>
        </el-table-column>
        
        <el-table-column prop="termValue" label="Срок займа" width="150" sortable>
          <template #default="{ row }">
            {{ row.termValue }} {{ row.termUnit === 0 ? 'дн.' : 'мес.' }}
          </template>
        </el-table-column>
        
        <el-table-column prop="interestValue" label="Процентная ставка" width="180" sortable>
          <template #default="{ row }">
            {{ row.interestValue }}%
          </template>
        </el-table-column>
        
        <el-table-column prop="status" label="Статус" width="200">
          <template #default="{ row }">
            <el-tag 
              :type="row.status === 0 ? 'success' : 'info'"
              effect="dark"
            >
              {{ getStatusText(row.status) }}
            </el-tag>
          </template>
        </el-table-column>
        
        <el-table-column prop="createdAt" label="Дата создания" width="180" sortable>
          <template #default="{ row }">
            {{ formatDate(row.createdAt) }}
          </template>
        </el-table-column>
        
        <el-table-column label="Действия" width="200" fixed="right">
          <template #default="{ row }">
            <el-button
              :type="row.status === 0 ? 'warning' : 'success'"
              size="small"
              @click="toggleStatus(row)"
              :loading="row.loading"
            >
              {{ row.status === 0 ? 'Снять с публикации' : 'Опубликовать' }}
            </el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { ElMessage } from 'element-plus'
import { Plus, RefreshRight } from '@element-plus/icons-vue'
import { loanService } from '../services/api'

const loans = ref([])
const loading = ref(false)

const filters = ref({
  status: null,
  minAmount: null,
  maxAmount: null,
  minTerm: null,
  maxTerm: null
})

const fetchLoans = async () => {
  try {
    loading.value = true
    const data = await loanService.getLoans(filters.value)
    loans.value = data.map(loan => ({ ...loan, loading: false }))
  } catch (error) {
    ElMessage.error('Ошибка при загрузке заявок: ' + (error.response?.data || error.message))
  } finally {
    loading.value = false
  }
}

const applyFilters = () => {
  fetchLoans()
}

const resetFilters = () => {
  filters.value = {
    status: null,
    minAmount: null,
    maxAmount: null,
    minTerm: null,
    maxTerm: null
  }
  fetchLoans()
}

const toggleStatus = async (loan) => {
  try {
    loan.loading = true
    await loanService.toggleLoanStatus(loan.id)
    
    ElMessage.success('Статус заявки успешно изменен')
    
    await fetchLoans()
  } catch (error) {
    loan.loading = false
    ElMessage.error('Ошибка при изменении статуса: ' + (error.response?.data || error.message))
  }
}

const getStatusText = (status) => {
  return status === 0 ? 'Опубликована' : 'Снята с публикации'
}

const formatCurrency = (amount) => {
  return new Intl.NumberFormat('ru-RU', {
    style: 'currency',
    currency: 'RUB'
  }).format(amount)
}

const formatDate = (dateString) => {
  return new Date(dateString).toLocaleString('ru-RU', {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour: '2-digit',
    minute: '2-digit'
  })
}

onMounted(() => {
  fetchLoans()
})
</script>

<style scoped>
.loan-list {
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.filter-card {
  margin-bottom: 20px;
}

.table-card {
  flex: 1;
}

:deep(.el-input-number) {
  width: 100%;
}

:deep(.el-select) {
  width: 100%;
}
</style>

