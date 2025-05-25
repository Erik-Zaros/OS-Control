<template>
  <q-page class="q-pa-md bg-grey-2 dark:bg-dark">
    <div class="text-h6 q-mb-md">Cadastro de Ticket</div>

    <FormTicket @os-cadastrada="voltar" />
  </q-page>
</template>

<script setup>
import FormTicket from 'src/components/tickets/FormTicket.vue'
import { useRouter } from 'vue-router'

const router = useRouter()
function voltar() {
  router.push('/consulta-os')
}
</script>