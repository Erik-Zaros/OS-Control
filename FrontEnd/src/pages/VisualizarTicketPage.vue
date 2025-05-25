<template>
  <q-page class="q-pa-md bg-grey-3">
    <q-card class="bg-white shadow-2" style="border: 1px solid #ccc;">

      <div class="text-center bg-grey-2 q-pa-md" style="border-bottom: 2px solid #ccc;">
        <div class="text-caption">Ticket Fabricante</div>
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
import { api } from 'boot/axios'

const route = useRoute()
const numeroOS = route.params.numero

const os = ref({})

onMounted(async () => {
  try {
    const { data } = await api.get(`/ticket/${numeroOS}`)
    os.value = {
      numero: data.id,
      nome_consumidor: data.cliente?.nome || '-',
      cpf_consumidor: data.cliente?.cpfCnpj || '-',
      produto: data.titulo || '-',
      data_abertura: formatarData(data.dataAbertura),
      status: statusLabel(data.status)
    }
  } catch (error) {
    os.value = { numero: numeroOS }
    throw error;
  }
})

function statusLabel(status) {
  switch (status) {
    case 0: return 'Aberto'
    case 1: return 'Em Andamento'
    case 2: return 'Concluído'
    default: return status
  }
}

function formatarData(data) {
  if (!data) return '-'
  const d = new Date(data)
  return d.toLocaleDateString('pt-BR')
}
</script>

<style scoped>
.bordered-cell {
  border-right: 1px solid #eee;
}

.bordered-cell:last-child {
  border-right: none;
}
</style>