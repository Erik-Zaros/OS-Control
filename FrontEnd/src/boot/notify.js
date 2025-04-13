import { Notify } from 'quasar'
import { boot } from 'quasar/wrappers'

export default boot(() => {
  const configBase = {
    position: 'top-right',
    timeout: 4000,
    classes: 'text-body1 q-pa-md',
    html: true,
    progress: true,
    actions: [{ icon: 'close', color: 'white' }]
  }

  window.sucesso = (mensagem) => {
    Notify.create({
      ...configBase,
      color: 'positive',
      icon: 'check_circle',
      message: mensagem
    })
  }

  window.erro = (mensagem) => {
    Notify.create({
      ...configBase,
      color: 'negative',
      icon: 'error',
      message: mensagem
    })
  }

  window.atencao = (mensagem) => {
    Notify.create({
      ...configBase,
      color: 'warning',
      icon: 'warning',
      message: mensagem
    })
  }
})
