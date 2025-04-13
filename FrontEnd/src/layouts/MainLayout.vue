<template>
  <q-layout view="lHh Lpr lFf">
    <q-header elevated class="bg-primary text-white">
      <q-toolbar>
        <q-btn flat dense round icon="menu" @click="toggleMini" />
        <q-toolbar-title>{{ pageTitle }}</q-toolbar-title>
        <q-space />
        <q-btn flat dense round :icon="$q.dark.isActive ? 'light_mode' : 'dark_mode'" @click="toggleDarkMode" />
        <q-btn flat dense round icon="account_circle" @click="perfilDialog = true" />
      </q-toolbar>
    </q-header>

    <q-drawer
      v-model="leftDrawerOpen"
      show-if-above
      :mini="miniState && !isHovering"
      @mouseover="isHovering = true"
      @mouseout="isHovering = false"
      bordered
    >
      <div class="q-pa-md flex flex-center">
        <q-img
          :src="logoSidebar"
          spinner-color="primary"
          style="width: 100%; max-width: 120px"
        />
      </div>

      <q-list class="drawer-list">
        <q-item clickable v-ripple to="/" exact>
          <q-item-section avatar>
            <q-icon name="dashboard" />
          </q-item-section>
          <q-item-section v-if="!miniState || isHovering">
            <span class="drawer-text">Dashboard</span>
          </q-item-section>
        </q-item>
        <q-item clickable v-ripple to="/clientes">
          <q-item-section avatar>
            <q-icon name="people" />
          </q-item-section>
          <q-item-section v-if="!miniState || isHovering">
            <span class="drawer-text">Clientes</span>
          </q-item-section>
        </q-item>
      </q-list>
    </q-drawer>

    <q-page-container>
      <router-view />
    </q-page-container>

    <q-dialog v-model="perfilDialog">
      <q-card style="min-width: 350px">
        <q-card-section class="row items-center q-pb-none">
          <div class="text-h6">Perfil do Usuário</div>
          <q-space />
          <q-btn icon="close" flat round dense v-close-popup />
        </q-card-section>
        <q-card-section>
          <q-list bordered>
            <q-item>
              <q-item-section avatar>
                <q-icon name="person" />
              </q-item-section>
              <q-item-section>
                <q-item-label>Erik Delanda Zaros</q-item-label>
                <q-item-label caption>erik@example.com</q-item-label>
              </q-item-section>
            </q-item>
            <q-item>
              <q-item-section avatar>
                <q-icon name="work" />
              </q-item-section>
              <q-item-section>
                <q-item-label>Tipo: Operador</q-item-label>
              </q-item-section>
            </q-item>
          </q-list>
        </q-card-section>
      </q-card>
    </q-dialog>
  </q-layout>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useQuasar } from 'quasar'
import { useRoute } from 'vue-router'

import logoLight from 'src/assets/serviceCore.png'
import logoDark from 'src/assets/serviceCoreDark.png'
import logoMiniLight from 'src/assets/serviceCoreMenor.png'
import logoMiniDark from 'src/assets/serviceCoreMenorDark.png'

const $q = useQuasar()
const route = useRoute()

const leftDrawerOpen = ref(true)
const miniState = ref(false)
const isHovering = ref(false)
const perfilDialog = ref(false)

function toggleMini() {
  miniState.value = !miniState.value
}

function toggleDarkMode() {
  $q.dark.toggle()
}

const pageTitle = computed(() => {
  switch (route.path) {
    case '/': return 'Menu'
    case '/clientes': return 'Cadastro de Cliente'
    case '/produtos': return 'Cadastro de Produto'
    default: return 'Projeto OS'
  }
})

const logoSidebar = computed(() => {
  if ($q.dark.isActive) {
    return miniState.value && !isHovering.value ? logoMiniDark : logoDark
  } else {
    return miniState.value && !isHovering.value ? logoMiniLight : logoLight
  }
})
</script>

<style scoped>
.q-drawer {
  width: 180px !important;
}
.q-toolbar-title {
  font-weight: bold;
}
.drawer-text {
  font-size: 16px;
  font-weight: 500;
}
</style>