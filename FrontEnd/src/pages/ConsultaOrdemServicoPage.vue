<template>
    <q-page class="q-pa-md bg-grey-2 dark:bg-dark">
      <div class="text-h6 q-mb-md">Consulta de Ordens de Serviço</div>
  
      <tabela-ordem-servico :ordens="ordens" @filtrar="filtrarOrdens" />
    </q-page>
</template>
  
  <script setup>
  import { ref } from 'vue'
  import TabelaOrdemServico from 'src/components/os/TabelaOrdemServico.vue'
  
  //const ordens = ref([])

  const ordens = ref([
  {
    numero: '1001',
    nome_consumidor: 'Carlos Oliveira',
    cpf_consumidor: '12345678900',
    produto: 'Geladeira Frost Free',
    data_abertura: '2025-05-01',
    status: 'Pendente'
  },
  {
    numero: '1002',
    nome_consumidor: 'Ana Souza',
    cpf_consumidor: '98765432100',
    produto: 'Micro-ondas 30L',
    data_abertura: '2025-05-03',
    status: 'Agendada'
  }
])
  
  //async function filtrarOrdens(filtros) {
  //  const res = await axios.post('/api/os/filtrar', filtros)
  //  ordens.value = res.data || []
  //}
  </script>
  