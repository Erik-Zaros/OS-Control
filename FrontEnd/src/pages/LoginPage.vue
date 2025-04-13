<template>
  <div :class="['fullscreen', 'flex', 'flex-center', darkMode ? 'q-dark bg-dark' : 'bg-grey-2']">
    <q-card class="login-card">
      <q-card-section class="text-center">
        <img :src="darkMode ? logoDark : logoLight" alt="logo" class="logo-img" />
        <div class="text-h5 text-primary">Bem-vindo ao OS Control</div>
        <div class="text-subtitle2 text-grey">Acesse sua conta abaixo</div>
      </q-card-section>

      <q-form @submit.prevent="handleLogin">
        <q-card-section>
          <q-input
            v-model="form.login"
            label="Usuário"
            dense
            outlined
            color="primary"
            class="q-mb-md"
            :prepend-icon="'person'"
            required
          />
          <q-input
            v-model="form.senha"
            label="Senha"
            type="password"
            dense
            outlined
            color="primary"
            class="q-mb-md"
            :prepend-icon="'lock'"
            required
          />

          <div class="text-subtitle2 text-grey-8 q-mb-sm">Tipo de Usuário</div>
          <q-option-group
            v-model="form.tipoUsuario"
            :options="tipos"
            type="radio"
            color="primary"
            inline
          />
        </q-card-section>

        <q-card-actions vertical align="center">
          <q-btn label="Entrar" type="submit" color="primary" class="full-width" />
          <q-toggle v-model="darkMode" label="Modo Escuro" class="q-mt-md" />
          <q-btn flat label="Criar conta" to="/cadastro-usuario" class="q-mt-sm text-primary" />
          <q-btn flat label="Ir para o sistema" to="/" class="q-mt-sm text-primary" />
        </q-card-actions>
      </q-form>
    </q-card>
  </div>
</template>

<script setup>
import { ref, watch } from 'vue'
import { useQuasar } from 'quasar'
import axios from 'boot/axios'
import logoLight from 'src/assets/serviceCore.png'
import logoDark from 'src/assets/serviceCoreDark.png'

const $q = useQuasar()

const form = ref({
  login: '',
  senha: '',
  tipoUsuario: 0,
})

const tipos = [
  { label: 'Operador', value: 0, icon: 'manage_accounts' },
  { label: 'Técnico', value: 1, icon: 'engineering' },
]

const darkMode = ref($q.dark.isActive)
watch(darkMode, (val) => {
  $q.dark.set(val)
})

const handleLogin = async () => {
  try {
    const response = await axios.post('/api/Usuario/login', {
      login: form.value.login,
      senha: form.value.senha
    })
    $q.notify({ type: 'positive', message: 'Login realizado com sucesso!' })
    console.log('Usuário logado:', response.data)
  } catch (error) {
    $q.notify({ type: 'negative', message: 'Falha no login!' })
    console.error(error)
  }
}
</script>

<style scoped>
.login-card {
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
