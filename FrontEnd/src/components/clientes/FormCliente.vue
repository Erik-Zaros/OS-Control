<template>
  <q-form @submit.prevent="handleSubmit">
    <loading-overlay :loading="loading" />

    <q-card class="q-pa-md" :class="$q.dark.isActive ? 'bg-dark text-white' : 'bg-white'">
      <div class="row q-col-gutter-md">
        <div class="col-md-6 col-12">
          <q-input v-model="cliente.cpfCnpj" label="CPF/CNPJ" outlined dense required maxlength="14" />
        </div>
        <div class="col-md-6 col-12">
          <q-input v-model="cliente.nome" label="Nome" outlined dense required maxlength="80" />
        </div>
        <div class="col-md-4 col-12">
          <q-input v-model="cliente.cep" label="CEP" outlined dense maxlength="9" />
        </div>
        <div class="col-md-8 col-12">
          <q-input v-model="cliente.endereco" label="Endereço" outlined dense />
        </div>
        <div class="col-md-4 col-12">
          <q-input v-model="cliente.bairro" label="Bairro" outlined dense />
        </div>
        <div class="col-md-4 col-12">
          <q-input v-model="cliente.numero" label="Número" outlined dense />
        </div>
        <div class="col-md-4 col-12">
          <q-input v-model="cliente.cidade" label="Cidade" outlined dense />
        </div>
        <div class="col-md-4 col-12">
          <q-input v-model="cliente.estado" label="Estado" outlined dense maxlength="10" />
        </div>
      </div>

      <div class="q-mt-md q-gutter-sm">
        <q-btn label="Buscar CEP" color="primary" @click="buscarCep" />
        <q-btn label="Gravar" type="submit" color="positive" />
      </div>
    </q-card>
  </q-form>
</template>

<script setup>
/* global sucesso, erro, atencao */
import { ref, watch } from 'vue'
// Importando axios diretamente do módulo axios
import * as axiosModule from 'axios'
// Importando a instância api
import { api } from 'boot/axios'
import LoadingOverlay from 'src/components/shared/LoadingOverlay.vue'

// Criando uma instância isolada do axios para chamadas externas
const axiosExternal = axiosModule.default.create()

const emit = defineEmits(['cliente-cadastrado'])

const props = defineProps({
  clienteParaEditar: {
    type: Object,
    default: null
  }
})

const loading = ref(false)

// Definir resetForm antes de usá-lo no watch
const resetForm = () => {
  cliente.value = {
    id: null,
    cpfCnpj: '',
    nome: '',
    cep: '',
    endereco: '',
    bairro: '',
    numero: '',
    cidade: '',
    estado: '',
    criadoEm: null
  }
}

const cliente = ref({
  id: null,
  cpfCnpj: '',
  nome: '',
  cep: '',
  endereco: '',
  bairro: '',
  numero: '',
  cidade: '',
  estado: '',
  criadoEm: null
})

watch(() => props.clienteParaEditar, (val) => {
  if (val) {
    cliente.value = {
      id: val.id || null,
      cpfCnpj: val.cpfCnpj || '',
      nome: val.nome || '',
      cep: val.cep || '',
      endereco: val.endereco || '',
      bairro: val.bairro || '',
      numero: val.numero || '',
      cidade: val.cidade || '',
      estado: val.estado || '',
      criadoEm: val.criadoEm || new Date().toISOString()
    }
    console.log('Cliente para editar:', cliente.value)
  } else {
    resetForm()
  }
}, { immediate: true })

const buscarCep = async () => {
  const cep = cliente.value.cep.replace(/\D/g, '')

  if (cep.length !== 8) {
    atencao('CEP deve conter exatamente 8 dígitos numéricos')
    return
  }

  loading.value = true
  try {
    // Usando a instância externa do axios para o ViaCEP
    const { data } = await axiosExternal.get(`https://viacep.com.br/ws/${cep}/json/`)
    if (data.erro) {
      erro('CEP inválido. Não foi possível encontrar esse CEP.')
    } else {
      cliente.value.endereco = data.logradouro || ''
      cliente.value.bairro = data.bairro || ''
      cliente.value.cidade = data.localidade || ''
      cliente.value.estado = data.uf || ''
    }
  } catch (error) {
    console.error('Erro ao buscar CEP:', error)
    erro('Erro ao buscar o CEP. Verifique a conexão com a internet.')
  } finally {
    loading.value = false
  }
}

const handleSubmit = async () => {
  loading.value = true
  try {
    const clienteData = {
      id: cliente.value.id,
      cpfCnpj: cliente.value.cpfCnpj,
      nome: cliente.value.nome,
      cep: cliente.value.cep,
      endereco: cliente.value.endereco,
      bairro: cliente.value.bairro,
      numero: cliente.value.numero,
      cidade: cliente.value.cidade,
      estado: cliente.value.estado
    }

    let response
    if (cliente.value.id) {
      response = await api.put(`/Cliente/${cliente.value.id}`, clienteData)
    } else {
      delete clienteData.id
      response = await api.post('/Cliente', clienteData)
    }

    if (response.status === 200 || response.status === 201) {
      sucesso(cliente.value.id ? "Cliente atualizado com sucesso!" : "Cliente cadastrado com sucesso!")
      resetForm()
      emit('cliente-cadastrado')
    } else {
      throw new Error('Operação não concluída com sucesso')
    }
  } catch (error) {
    console.error('Erro completo:', error)
    if (error.response) {
      console.error('Detalhes da resposta:', error.response.data)
    }
    erro('Erro ao cadastrar/editar cliente!')
  } finally {
    loading.value = false
  }
}
</script>