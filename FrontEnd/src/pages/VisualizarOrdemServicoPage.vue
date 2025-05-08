<template>
    <q-page class="q-pa-md bg-grey-3">
      <q-card class="bg-white shadow-2" style="border: 1px solid #ccc;">

        <div class="text-center bg-grey-2 q-pa-md" style="border-bottom: 2px solid #ccc;">
          <div class="text-caption">OS Fabricante</div>
          <div class="text-h2 text-orange-10 text-bold">{{ os.numero }}</div>
          <q-badge color="warning" class="q-mt-sm" outline>{{ os.status }}</q-badge>
        </div>
  
        <div class="row q-col-gutter-xs q-ma-none" style="border-bottom: 1px solid #ccc;">
          <div class="col-md-6 col-12 q-pa-sm bordered-cell">
            <div class="text-bold text-grey-8">Cliente</div>
            <div>{{ os.nome_consumidor }}</div>
          </div>
          <div class="col-md-6 col-12 q-pa-sm bordered-cell">
            <div class="text-bold text-grey-8">CPF</div>
            <div>{{ os.cpf_consumidor }}</div>
          </div>
        </div>
  
        <div class="row q-col-gutter-xs q-ma-none" style="border-bottom: 1px solid #ccc;">
          <div class="col-md-8 col-12 q-pa-sm bordered-cell">
            <div class="text-bold text-grey-8">Produto</div>
            <div>{{ os.produto }}</div>
          </div>
          <div class="col-md-4 col-12 q-pa-sm bordered-cell">
            <div class="text-bold text-grey-8">Data de Abertura</div>
            <div>{{ os.data_abertura }}</div>
          </div>
        </div>
  
      </q-card>
    </q-page>
  </template>

  <script setup>
  import { ref, onMounted } from 'vue'
  import { useRoute } from 'vue-router'
  
  const route = useRoute()
  const numeroOS = route.params.numero
  
  const os = ref({})

  onMounted(() => {
    os.value = {
      numero: numeroOS,
      nome_consumidor: 'ROSANGELA ALVES DE SOUZA',
      cpf_consumidor: '246.472.948-54',
      produto: 'WAP DESOBSTRUADORA ECO FIT ULTRA 2200 127V',
      data_abertura: '06/05/2025',
      status: 'AGUARDANDO PEÇAS'
    }
  })
  </script>

<style scoped>
.bordered-cell {
  border-right: 1px solid #eee;
}
.bordered-cell:last-child {
  border-right: none;
}
</style>

  