<template>
  <q-form @submit.prevent="handleSubmit">
    <loading-overlay :loading="loading" />

    <q-card class="q-pa-md" :class="$q.dark.isActive ? 'bg-dark text-white' : 'bg-white'">
      <div class="row q-col-gutter-md">
        <div class="col-md-6 col-12">
          <q-input v-model="usuario.nome" label="Nome Completo" outlined dense required maxlength="80" />
        </div>
        <div class="col-md-6 col-12">
          <q-input v-model="usuario.funcao" label="Função" outlined dense maxlength="40" />
        </div>
        <div class="col-md-4 col-12">
          <q-input v-model="usuario.login" label="Login" outlined dense required maxlength="40" />
        </div>
        <div class="col-md-4 col-12">
          <q-input v-model="usuario.email" label="E-mail" outlined dense type="email" maxlength="80" />
        </div>
        <div class="col-md-4 col-12">
          <q-input v-model="usuario.telefone" label="Telefone" outlined dense mask="(##) #####-####" maxlength="15" />
        </div>
        <div class="col-md-4 col-12">
          <q-select
            v-model="usuario.tipousuario"
            :options="tiposUsuario"
            label="Tipo de Usuário"
            outlined
            dense
            emit-value
            map-options
          />
        </div>
        <div class="col-md-4 col-12">
          <q-input v-model="usuario.especialidade" label="Especialidade" outlined dense maxlength="40" />
        </div>
        <div class="col-md-4 col-12">
          <q-input v-model="usuario.senha" label="Senha" outlined dense type="password" maxlength="20" />
        </div>
        <div class="col-md-4 col-12">
          <q-input v-model="usuario.senha2" label="Repita a senha" outlined dense type="password" maxlength="20" />
        </div>
      </div>

      <div class="q-mt-md q-gutter-sm row items-center">
        <q-toggle v-model="usuario.ativo" label="Ativo" color="primary" />
        <q-btn label="Cadastrar" type="submit" color="primary" icon="add" class="q-ml-sm" />
      </div>
    </q-card>
  </q-form>
</template>

<script setup>
import { ref, watch } from 'vue'
import LoadingOverlay from 'src/components/shared/LoadingOverlay.vue'

const emit = defineEmits(['usuario-cadastrado'])

const props = defineProps({
  usuarioParaEditar: {
    type: Object,
    default: null
  }
})

const loading = ref(false)
const tiposUsuario = [
  { label: 'Técnico', value: 'Técnico' },
  { label: 'Administrador', value: 'Administrador' },
  { label: 'Operador', value: 'Operador' },
  { label: 'TI', value: 'TI' }
]

const resetForm = () => {
  usuario.value = {
    id: null,
    nome: '',
    funcao: '',
    login: '',
    senha: '',
    senha2: '',
    email: '',
    telefone: '',
    tipousuario: '',
    ultimoacesso: null,
    especialidade: '',
    ativo: true
  }
}

const usuario = ref({
  id: null,
  nome: '',
  funcao: '',
  login: '',
  senha: '',
  senha2: '',
  email: '',
  telefone: '',
  tipousuario: '',
  ultimoacesso: null,
  especialidade: '',
  ativo: true
})

watch(() => props.usuarioParaEditar, (val) => {
  if (val) {
    usuario.value = {
      id: val.id || null,
      nome: val.nome || '',
      funcao: val.funcao || '',
      login: val.login || '',
      senha: '',
      senha2: '',
      email: val.email || '',
      telefone: val.telefone || '',
      tipousuario: val.tipousuario || '',
      ultimoacesso: val.ultimoacesso || null,
      especialidade: val.especialidade || '',
      ativo: val.ativo ?? true
    }
  } else {
    resetForm()
  }
}, { immediate: true })

const handleSubmit = () => {
  loading.value = true

  setTimeout(() => {
    emit('usuario-cadastrado', { ...usuario.value, id: usuario.value.id ?? Date.now(), ultimoacesso: usuario.value.ultimoacesso ?? new Date().toISOString() })
    resetForm()
    loading.value = false
  }, 800)
}
</script>
