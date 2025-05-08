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

        </q-card-section>

        <q-card-actions vertical align="center">
          <q-btn label="Entrar" type="submit" color="primary" class="full-width" />
          <q-toggle v-model="darkMode" label="Modo Escuro" class="q-mt-md" />
          <q-btn flat label="Criar conta" to="/cadastro-usuario" class="q-mt-sm text-primary" />
          <!-- <q-btn flat label="Ir para o sistema" to="/dashboard" class="q-mt-sm text-primary" /> -->
          <!-- desenvolver tela de planos -->
          <!--
          <p class="text-grey-8 q-mb-sm">Ainda não adquiriu o nosso sistema?</p>
          <q-btn
            flat
            label="Conheça nossos planos"
            to="/planos"
            icon="shopping_cart"
            color="primary"
          />
          -->
        </q-card-actions>
      </q-form>
    </q-card>
  </div>
</template>

<script setup>
import { ref, watch } from 'vue'
import { useQuasar } from 'quasar'
import { api} from 'boot/axios'
import { useRouter, useRoute } from 'vue-router'
import logoLight from 'src/assets/serviceCore.png'
import logoDark from 'src/assets/serviceCoreDark.png'

const $q = useQuasar()
const router = useRouter()
const route = useRoute()

const form = ref({
  login: '',
  senha: '',
  tipoUsuario: 0,
})

const darkMode = ref($q.dark.isActive)
watch(darkMode, (val) => {
  $q.dark.set(val)
})

const handleLogin = async () => {
  try {
    const response = await api.post('/Usuario/login', {
      login: form.value.login,
      senha: form.value.senha
    })
    
    localStorage.clear()
    
    const userData = response.data.data.usuario
    const token = response.data.data.token
    
    localStorage.setItem('usuario', JSON.stringify(userData))
    localStorage.setItem('token', token)
    
    $q.notify({ type: 'positive', message: 'Login realizado com sucesso!' })
    
    const redirectPath = route.query.redirect || '/dashboard'
    router.push(redirectPath)
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
