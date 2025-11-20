<template>
  <div class="loan-create">
    <el-card>
      <template #header>
        <div class="card-header">
          <span>Создание новой заявки</span>
          <el-button @click="$router.push('/')" :icon="Back">
            Назад к списку
          </el-button>
        </div>
      </template>

      <el-form
        ref="formRef"
        :model="form"
        :rules="rules"
        label-width="180px"
        label-position="left"
        @submit.prevent="onSubmit"
      >
        <el-form-item label="Номер заявки" prop="number">
          <el-input
            v-model="form.number"
            placeholder="Введите уникальный номер заявки"
            clearable
          />
        </el-form-item>

        <el-form-item label="Сумма займа" prop="amount">
          <el-input-number
            v-model="form.amount"
            :min="0.01"
            :precision="2"
            :step="1000"
            placeholder="Введите сумму"
            style="width: 100%"
          />
          <template #extra>
            <span class="form-hint">Сумма должна быть больше 0</span>
          </template>
        </el-form-item>

        <el-form-item label="Срок займа" prop="termValue">
          <el-row :gutter="10" style="width: 100%">
            <el-col :span="16">
              <el-input-number
                v-model="form.termValue"
                :min="1"
                :step="1"
                placeholder="Введите срок"
                style="width: 100%"
              />
            </el-col>
            <el-col :span="8">
              <el-select
                v-model="form.termUnit"
                placeholder="Единица"
                style="width: 100%"
              >
                <el-option label="Дней" :value="0" />
                <el-option label="Месяцев" :value="1" />
              </el-select>
            </el-col>
          </el-row>
          <template #extra>
            <span class="form-hint">Срок займа должен быть больше 0</span>
          </template>
        </el-form-item>

        <el-form-item label="Процентная ставка (%)" prop="interestValue">
          <el-input-number
            v-model="form.interestValue"
            :min="0.01"
            :max="100"
            :precision="2"
            :step="0.5"
            placeholder="Введите процентную ставку"
            style="width: 100%"
          />
          <template #extra>
            <span class="form-hint">Процентная ставка должна быть больше 0</span>
          </template>
        </el-form-item>

        <el-form-item class="button-group">
          <el-button 
            type="primary" 
            @click="onSubmit" 
            :loading="loading"
            size="large"
          >
            Создать заявку
          </el-button>
          <el-button 
            @click="resetForm" 
            size="large"
          >
            Очистить форму
          </el-button>
        </el-form-item>
      </el-form>

      <el-divider />

      <div class="preview-section" v-if="isFormFilled">
        <h3>Предварительный просмотр</h3>
        <el-descriptions :column="2" border>
          <el-descriptions-item label="Номер заявки">
            {{ form.number || '—' }}
          </el-descriptions-item>
          <el-descriptions-item label="Статус">
            <el-tag type="success">Опубликована</el-tag>
          </el-descriptions-item>
          <el-descriptions-item label="Сумма займа">
            {{ form.amount ? formatCurrency(form.amount) : '—' }}
          </el-descriptions-item>
          <el-descriptions-item label="Срок займа">
            {{ form.termValue ? `${form.termValue} ${form.termUnit === 0 ? 'дн.' : 'мес.'}` : '—' }}
          </el-descriptions-item>
          <el-descriptions-item label="Процентная ставка">
            {{ form.interestValue ? `${form.interestValue}%` : '—' }}
          </el-descriptions-item>
        </el-descriptions>
      </div>
    </el-card>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import { ElMessage } from 'element-plus'
import { Back } from '@element-plus/icons-vue'
import { loanService } from '../services/api'

const router = useRouter()
const formRef = ref(null)
const loading = ref(false)

const form = ref({
  number: '',
  amount: null,
  termValue: null,
  termUnit: 0,
  interestValue: null
})

const rules = {
  number: [
    { required: true, message: 'Введите номер заявки', trigger: 'blur' },
    { min: 1, max: 50, message: 'Длина должна быть от 1 до 50 символов', trigger: 'blur' }
  ],
  amount: [
    { required: true, message: 'Введите сумму займа', trigger: 'blur' },
    { 
      validator: (rule, value, callback) => {
        if (!value || value <= 0) {
          callback(new Error('Сумма должна быть больше 0'))
        } else {
          callback()
        }
      },
      trigger: 'change'
    }
  ],
  termValue: [
    { required: true, message: 'Введите срок займа', trigger: 'blur' },
    {
      validator: (rule, value, callback) => {
        if (!value || value < 1) {
          callback(new Error('Срок займа должен быть больше 0'))
        } else {
          callback()
        }
      },
      trigger: 'change'
    }
  ],
  interestValue: [
    { required: true, message: 'Введите процентную ставку', trigger: 'blur' },
    {
      validator: (rule, value, callback) => {
        if (!value || value <= 0) {
          callback(new Error('Процентная ставка должна быть больше 0'))
        } else {
          callback()
        }
      },
      trigger: 'change'
    }
  ]
}

const isFormFilled = computed(() => {
  return form.value.number && 
         form.value.amount && 
         form.value.termValue && 
         form.value.interestValue
})

const onSubmit = async () => {
  if (!formRef.value) return

  try {
    await formRef.value.validate()
  } catch (error) {
    return
  }

  loading.value = true
  
  try {
    await loanService.createLoan(form.value)
    
    ElMessage.success('Заявка успешно создана!')
    
    setTimeout(() => {
      router.push('/')
    }, 500)
  } catch (error) {
    let errorMessage = 'Неизвестная ошибка'
    
    if (error.response?.data) {
      const data = error.response.data
      
      if (data.message) {
        errorMessage = data.message
        
        if (data.errors && typeof data.errors === 'object') {
          const errorsList = Object.entries(data.errors)
            .map(([field, messages]) => `${field}: ${Array.isArray(messages) ? messages.join(', ') : messages}`)
            .join('; ')
          errorMessage += '. ' + errorsList
        }
      } else if (typeof data === 'string') {
        errorMessage = data
      }
    } else if (error.message) {
      errorMessage = error.message
    }
    
    ElMessage.error('Ошибка при создании заявки: ' + errorMessage)
  } finally {
    loading.value = false
  }
}

const resetForm = () => {
  if (!formRef.value) return
  formRef.value.resetFields()
}

const formatCurrency = (amount) => {
  return new Intl.NumberFormat('ru-RU', {
    style: 'currency',
    currency: 'RUB'
  }).format(amount)
}
</script>

<style scoped>
.loan-create {
  max-width: 800px;
  margin: 0 auto;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.form-hint {
  color: #909399;
  font-size: 12px;
}

.preview-section {
  margin-top: 20px;
}

.preview-section h3 {
  margin-bottom: 15px;
  color: #303133;
}

:deep(.el-input-number) {
  width: 100%;
}

:deep(.el-form-item__content) {
  flex-direction: column;
  align-items: flex-start;
}

.button-group :deep(.el-form-item__content) {
  flex-direction: row !important;
  gap: 12px;
}
</style>

