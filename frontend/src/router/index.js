import { createRouter, createWebHistory } from 'vue-router'
import LoanList from '../views/LoanList.vue'
import LoanCreate from '../views/LoanCreate.vue'

const routes = [
  {
    path: '/',
    name: 'LoanList',
    component: LoanList
  },
  {
    path: '/create',
    name: 'LoanCreate',
    component: LoanCreate
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router

