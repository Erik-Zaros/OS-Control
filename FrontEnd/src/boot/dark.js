import { Dark } from 'quasar'

export default () => {
  const preferencia = localStorage.getItem('modo_escuro')
  if (preferencia === 'true') {
    Dark.set(true)
  } else {
    Dark.set(false)
  }
}
