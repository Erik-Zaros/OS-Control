<template>
    <div :class="['fullscreen', 'flex', 'flex-center', darkMode ? 'q-dark bg-dark' : 'bg-grey-2']">
      <q-card class="user-form-card">
        <q-card-section class="text-center">
          <img :src="logo" alt="logo" class="logo-img" />
          <div class="text-h6 text-primary">Cadastro de Usuário</div>
        </q-card-section>
  
        <q-form @submit.prevent="handleSubmit">
          <q-card-section>
            <q-input v-model="form.nome" label="Nome" dense outlined color="primary" class="q-mb-md" />
            <q-input v-model="form.login" label="Login" dense outlined color="primary" class="q-mb-md" />
            <q-input v-model="form.senha" label="Senha" type="password" dense outlined color="primary" class="q-mb-md" />
            <q-input v-model="form.email" label="E-mail" dense outlined color="primary" class="q-mb-md" />
            <q-input v-model="form.telefone" label="Telefone" dense outlined color="primary" class="q-mb-md" />
            <q-input v-model="form.especialidade" label="Especialidade" dense outlined color="primary" class="q-mb-md" />
  
            <div class="q-mb-md">
              <div class="text-subtitle2 text-grey-8">Tipo de Usuário</div>
              <q-option-group
                v-model="form.tipoUsuario"
                :options="[
                  { label: 'Operador', value: 0 },
                  { label: 'Técnico', value: 1 }
                ]"
                type="radio"
                color="primary"
                inline
              />
            </div>
  
            <q-toggle v-model="form.ativo" label="Ativo" color="primary" />
          </q-card-section>
  
          <q-card-actions align="center">
            <q-btn label="Cadastrar" color="primary" type="submit" class="full-width" />
          </q-card-actions>
          <q-card-section class="text-center q-mt-sm">
            <q-btn flat label="Já tem uma conta? Faça login" to="/login" class="text-primary" />
          </q-card-section>
        </q-form>
      </q-card>
    </div>
  </template>
  
  <script setup>
  import { ref, computed } from 'vue'
  import axios from 'boot/axios'
  import { useQuasar } from 'quasar'
  import serviceCoreLight from 'src/assets/serviceCore.png'
  import serviceCoreDark from 'src/assets/serviceCoreDark.png'
  
  const $q = useQuasar()
  const darkMode = ref($q.dark.isActive)
  
  const logo = computed(() => darkMode.value ? serviceCoreDark : serviceCoreLight)
  
  const form = ref({
    nome: '',
    login: '',
    senha: '',
    email: '',
    telefone: '',
    especialidade: '',
    tipoUsuario: 0,
    ativo: true
  })
  
  const handleSubmit = async () => {
    try {
      const response = await axios.post('/api/Usuario/criar-usuario', form.value)
      $q.notify({ type: 'positive', message: 'Usuário cadastrado com sucesso!' })
      console.log(response.data)
    } catch (error) {
      $q.notify({ type: 'negative', message: 'Erro ao cadastrar usuário!' })
      console.error(error)
    }
  }
  </script>
  
  <style scoped>
  .user-form-card {
    width: 100%;
    max-width: 420px;
    padding: 16px;
    transition: all 0.3s ease-in-out;
  }
  
  .logo-img {
    max-width: 180px;
    height: auto;
    display: block;
    margin: 0 auto 16px auto;
  }
  </style>
  