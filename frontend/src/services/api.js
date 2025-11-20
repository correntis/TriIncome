import axios from 'axios'

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api'

const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json'
  }
})

export const loanService = {
  async getLoans(filters = {}) {
    const params = {}
    
    if (filters.status !== null && filters.status !== undefined) {
      params.status = filters.status
    }
    if (filters.minAmount) {
      params.minAmount = filters.minAmount
    }
    if (filters.maxAmount) {
      params.maxAmount = filters.maxAmount
    }
    if (filters.minTerm) {
      params.minTerm = filters.minTerm
    }
    if (filters.maxTerm) {
      params.maxTerm = filters.maxTerm
    }

    const response = await apiClient.get('/loans', { params })
    return response.data
  },

  async getLoan(id) {
    const response = await apiClient.get(`/loans/${id}`)
    return response.data
  },

  async createLoan(loanData) {
    const response = await apiClient.post('/loans', loanData)
    return response.data
  },

  async toggleLoanStatus(id) {
    const response = await apiClient.patch(`/loans/${id}/toggle-status`)
    return response.data
  }
}

export default apiClient

