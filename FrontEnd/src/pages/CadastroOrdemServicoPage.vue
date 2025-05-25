<template>
  <q-page class="q-pa-md bg-grey-2 dark:bg-dark">
    <div class="text-h6 q-mb-md">Cadastro de Ordem de Serviço</div>

    <form-ordem-servico @os-cadastrada="voltar" />
  </q-page>
</template>

<script setup>
import FormOrdemServico from 'src/components/os/FormOrdemServico.vue'
import { useRouter } from 'vue-router'

const router = useRouter()
function voltar() {
  router.push('/consulta-os')
}
</script>