<template>
    <q-form @submit.prevent="handleSubmit">
      <loading-overlay :loading="loading" />
  
      <q-card class="q-pa-md q-mb-md" :class="$q.dark.isActive ? 'bg-dark text-white' : 'bg-white'">
        <div class="row q-col-gutter-md">
          <div class="col-md-6 col-12">
            <q-input v-model="os.numero" label="Número da OS" outlined dense readonly />
          </div>
          <div class="col-md-6 col-12">
            <q-input v-model="os.data_abertura" label="Data de Abertura" type="date" outlined dense required />
          </div>
          <div class="col-md-6 col-12">
            <q-input v-model="os.nome_consumidor" label="Nome do Consumidor" outlined dense required />
          </div>
          <div class="col-md-6 col-12">
            <q-input v-model="os.cpf_consumidor" label="CPF do Consumidor" outlined dense required maxlength="11" />
          </div>
          <div class="col-12">
            <q-select v-model="os.produto_id" :options="produtos" label="Produto" outlined dense required option-label="descricao" option-value="id" />
          </div>
        </div>
  
        <div class="q-mt-md q-gutter-sm">
          <q-btn label="Gravar" type="submit" color="positive" />
        </div>
      </q-card>
    </q-form>
  </template>
  
  <script setup>
  /* global sucesso, erro */
  import { ref, onMounted } from 'vue'
  import axios from 'boot/axios'
  import LoadingOverlay from 'src/components/shared/LoadingOverlay.vue'
  
  const emit = defineEmits(['os-cadastrada'])
  
  const os = ref({
    numero: '',
    data_abertura: '',
    nome_consumidor: '',
    cpf_consumidor: '',
    produto_id: ''
  })
  
  const produtos = ref([])
  const loading = ref(false)
  
  onMounted(async () => {
    const res = await axios.get('/api/produto/listar')
    produtos.value = res.data || []
  })
  
  const handleSubmit = async () => {
    loading.value = true
    try {
      await axios.post('/api/os', os.value)
      sucesso("Ordem de Serviço cadastrada!")
      emit('os-cadastrada')
    } catch {
      erro("Erro ao cadastrar OS!")
    } finally {
      loading.value = false
    }
  }
  </script>
  