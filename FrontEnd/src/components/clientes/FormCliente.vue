<template>
  <q-form @submit.prevent="handleSubmit">
    <loading-overlay :loading="loading" />

    <q-card class="q-pa-md" :class="$q.dark.isActive ? 'bg-dark text-white' : 'bg-white'">
      <div class="row q-col-gutter-md">
        <div class="col-md-6 col-12">
          <q-input v-model="cliente.cpf" label="CPF" outlined dense required maxlength="11" />
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
import axios from 'axios'
import LoadingOverlay from 'src/components/shared/LoadingOverlay.vue'

const emit = defineEmits(['cliente-cadastrado'])

const props = defineProps({
  clienteParaEditar: {
    type: Object,
    default: null
  }
})

const loading = ref(false)

const cliente = ref({
  cpf: '',
  nome: '',
  cep: '',
  endereco: '',
  bairro: '',
  numero: '',
  cidade: '',
  estado: ''
})

watch(() => props.clienteParaEditar, (val) => {
  if (val) {
    cliente.value = { ...val }
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
    const { data } = await axios.get(`https://viacep.com.br/ws/${cep}/json/`)
    if (data.erro) {
      erro('CEP inválido. Não foi possível encontrar esse CEP.')
    } else {
      cliente.value.endereco = data.logradouro || ''
      cliente.value.bairro = data.bairro || ''
      cliente.value.cidade = data.localidade || ''
      cliente.value.estado = data.uf || ''
    }
  } catch {
    erro('Erro ao buscar o CEP. Verifique a conexão com a internet.')
  } finally {
    loading.value = false
  }
}

const handleSubmit = async () => {
  try {
    if (props.clienteParaEditar) {
      await axios.put(`/api/cliente/${cliente.value.cpf}`, cliente.value)
      sucesso("Cliente atualizado com sucesso!")
    } else {
      await axios.post('/api/cliente', cliente.value)
      sucesso("Cliente cadastrado com sucesso!")
    }

    emit('cliente-cadastrado')
    cliente.value = {
      cpf: '',
      nome: '',
      cep: '',
      endereco: '',
      bairro: '',
      numero: '',
      cidade: '',
      estado: ''
    }
  } catch {
    erro('Erro ao cadastrar/editar cliente!')
  }
}
</script>
