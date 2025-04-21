export const isAuthenticated = () => {
  const token = localStorage.getItem('token')
  return !!token
}

export const getToken = () => {
  return localStorage.getItem('token')
}

export const clearAuth = () => {
  localStorage.removeItem('token')
  localStorage.removeItem('usuario')
} 