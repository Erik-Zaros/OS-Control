<template>
  <q-form @submit.prevent="handleSubmit" ref="formRef">
    <loading-overlay :loading="props.loading" />

    <q-card class="q-pa-md" :class="$q.dark.isActive ? 'bg-dark text-white' : 'bg-white'">
      <q-card-section>
        <div class="text-h6">{{ isEdicao ? 'Editar Usuário' : 'Novo Usuário' }}</div>
      </q-card-section>

      <div class="row q-col-gutter-md">
        <div class="col-md-6 col-12">
          <q-input v-model="usuario.nome" label="Nome Completo *" outlined dense maxlength="100" />
        </div>

        <div class="col-md-6 col-12">
          <q-input v-model="usuario.email" label="E-mail *" outlined dense type="email" maxlength="150" :rules="[
          ]" />
        </div>

        <div class="col-md-4 col-12">
          <q-input v-model="usuario.login" label="Login *" outlined dense maxlength="50" />
        </div>

        <div class="col-md-4 col-12">
          <q-input v-model="usuario.telefone" label="Telefone" outlined dense mask="(##) #####-####" maxlength="20" />
        </div>

        <div class="col-md-4 col-12">
          <q-select v-model="usuario.tipoUsuario" :options="tiposUsuario" label="Tipo de Usuário *" outlined dense
            emit-value map-options />
        </div>

        <div class="col-md-6 col-12" v-if="!isEdicao">
          <q-input v-model="usuario.senha" label="Senha *" outlined dense type="password" maxlength="255" />
        </div>

        <div class="col-md-6 col-12" v-if="!isEdicao">
          <q-input v-model="usuario.senha2" label="Repita a senha *" outlined dense type="password" maxlength="255"
            :rules="[
              val => val === usuario.senha || 'Senhas não coincidem'
            ]" />
        </div>

        <!-- <div class="col-12" v-if="isEdicao">
          <q-checkbox v-model="alterarSenha" label="Alterar senha" color="primary" />
        </div> -->

        <div class="col-md-6 col-12" v-if="isEdicao && alterarSenha">
          <q-input v-model="usuario.senha" label="Nova Senha *" outlined dense type="password" maxlength="255"
            :rules="[val => !alterarSenha]" />
        </div>

        <div class="col-md-6 col-12" v-if="isEdicao && alterarSenha">
          <q-input v-model="usuario.senha2" label="Repita a nova senha *" outlined dense type="password" maxlength="255"
            :rules="[
              val => !alterarSenha,
              val => !alterarSenha || val === usuario.senha || 'Senhas não coincidem'
            ]" />
        </div>
      </div>

      <div class="q-mt-md q-gutter-sm row items-center">
        <q-toggle v-model="usuario.ativo" label="Ativo" color="primary" />

        <q-space />

        <q-btn flat label="Cancelar" color="grey" @click="cancelar" :disable="loading" />

        <q-btn :label="isEdicao ? 'Atualizar' : 'Cadastrar'" type="submit" color="primary"
          :icon="isEdicao ? 'edit' : 'add'" :loading="loading" />
      </div>
    </q-card>
  </q-form>
</template>

<script setup>
import { ref, watch, computed } from 'vue'
import { useQuasar } from 'quasar'
import LoadingOverlay from 'src/components/shared/LoadingOverlay.vue'

const $q = useQuasar()
const emit = defineEmits(['usuario-cadastrado'])

const props = defineProps({
  usuarioParaEditar: {
    type: Object,
    default: null
  },
  loading: {
    type: Boolean,
    default: false
  }
})

const formRef = ref()
const alterarSenha = ref(false)

const tiposUsuario = [
  { label: 'Operador', value: 0 },
  { label: 'Técnico', value: 1 }
]

const resetForm = () => {
  usuario.value = {
    id: null,
    nome: '',
    login: '',
    senha: '',
    senha2: '',
    email: '',
    telefone: '',
    tipoUsuario: null,
    ativo: true
  }
  alterarSenha.value = false
}

const usuario = ref({
  id: null,
  nome: '',
  login: '',
  senha: '',
  senha2: '',
  email: '',
  telefone: '',
  tipoUsuario: null,
  ativo: true
})

const isEdicao = computed(() => !!usuario.value.id)

watch(() => props.usuarioParaEditar, (val) => {
  if (val) {
    usuario.value = {
      id: val.id || null,
      nome: val.nome || '',
      login: val.login || '',
      senha: '',
      senha2: '',
      email: val.email || '',
      telefone: val.telefone || '',
      tipoUsuario: val.tipoUsuario ?? null,
      ativo: val.ativo ?? true
    }
    alterarSenha.value = false
  } else {
    resetForm()
  }
}, { immediate: true })

const handleSubmit = async () => {
  try {
    const isValid = await formRef.value.validate()
    if (!isValid) {
      $q.notify({
        type: 'negative',
        message: 'Por favor, corrija os erros no formulário'
      })
      return
    }

    const dadosParaEnviar = {
      nome: usuario.value.nome,
      login: usuario.value.login,
      email: usuario.value.email,
      telefone: usuario.value.telefone,
      tipoUsuario: usuario.value.tipoUsuario,
      ativo: usuario.value.ativo
    }

    if (isEdicao.value && usuario.value.id) {
      dadosParaEnviar.id = usuario.value.id
    }

    if (!isEdicao.value || (isEdicao.value && alterarSenha.value)) {
      dadosParaEnviar.senha = usuario.value.senha
    }

    emit('usuario-cadastrado', dadosParaEnviar)

    if (!isEdicao.value) {
      resetForm()
      formRef.value.resetValidation()
    }

  } catch (error) {
    console.error('Erro no formulário:', error)
    $q.notify({
      type: 'negative',
      message: 'Erro ao processar formulário'
    })
  }
}

const cancelar = () => {
  resetForm()
  formRef.value.resetValidation()
}
</script>