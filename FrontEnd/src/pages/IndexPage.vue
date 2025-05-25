<template>
  <q-page class="q-pa-md bg-grey-2 dark:bg-dark">
    <div class="text-center q-mb-md">
      <q-btn label="Download Excel" color="green" outline size="sm" icon="file_download" @click="downloadExcel" />
    </div>

    <div class="row q-col-gutter-md">
      <div class="col-12 col-md-6">
        <q-card class="q-pa-md shadow-2">
          <div class="text-h6 text-center q-mb-sm">Gráfico de Pizza</div>
          <div ref="graficoPizza" class="grafico-container"></div>
        </q-card>
      </div>

      <div class="col-12 col-md-6">
        <q-card class="q-pa-md shadow-2">
          <div class="text-h6 text-center q-mb-sm">Status dos Produtos</div>
          <div ref="graficoStatusProduto" class="grafico-container"></div>
        </q-card>
      </div>

      <div class="col-12">
        <q-card class="q-pa-md shadow-2 q-mt-md">
          <div class="text-h6 text-center q-mb-sm">Gráfico de Colunas</div>
          <div ref="graficoColunas" class="grafico-container"></div>
        </q-card>
      </div>
    </div>

    <q-inner-loading :showing="loading">
      <q-spinner-gears size="50px" color="primary" />
    </q-inner-loading>
  </q-page>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue'
import Highcharts from 'highcharts'
import { useQuasar } from 'quasar'
import { api } from 'boot/axios'

const $q = useQuasar()
const graficoPizza = ref(null)
const graficoColunas = ref(null)
const graficoStatusProduto = ref(null)
const loading = ref(false)

const dados = ref({
  clientes: 0,
  produtos: 0,
  produtosAtivos: 0,
  ordens_servico: 0,
  tickets: 0,
  usuarios: 0,
  usuariosAtivos: 0
})

async function carregarDados() {
  loading.value = true
  try {
    const [
      clientesRes,
      produtosRes,
      ordensRes,
      ticketsRes,
      usuariosRes
    ] = await Promise.all([
      api.get('/Cliente/dashboard'),
      api.get('/Produto/dashboard'),
      api.get('/OrdemServico/dashboard'),
      api.get('/Ticket/dashboard'),
      api.get('/Usuario/dashboard')
    ])

    dados.value = {
      clientes: clientesRes.data.totalClientes || 0,
      produtos: produtosRes.data.totalProdutos || 0,
      produtosAtivos: produtosRes.data.produtosAtivos || 0,
      ordens_servico: ordensRes.data.totalOrdens || 0,
      tickets: ticketsRes.data.totalTickets || 0,
      usuarios: usuariosRes.data.totalUsuarios || 0,
      usuariosAtivos: usuariosRes.data.usuariosAtivos || 0
    }

    atualizarGraficos()
  } catch (error) {
    console.error('Erro ao carregar dados:', error)
    $q.notify({
      type: 'negative',
      message: 'Erro ao carregar dados do dashboard'
    })
  } finally {
    loading.value = false
  }
}

function atualizarGraficos() {
  const isDark = $q.dark.isActive
  const backgroundColor = isDark ? '#121212' : '#ffffff'
  const textColor = isDark ? '#ffffff' : '#000000'

  Highcharts.setOptions({
    chart: { backgroundColor },
    title: { style: { color: textColor } },
    subtitle: { style: { color: textColor } },
    xAxis: { labels: { style: { color: textColor } }, title: { style: { color: textColor } } },
    yAxis: { labels: { style: { color: textColor } }, title: { style: { color: textColor } } },
    legend: { itemStyle: { color: textColor } },
    tooltip: { style: { color: textColor } }
  })

  carregarGraficoPizza()
  carregarGraficoColunas()
  carregarGraficoStatusProduto()
}

onMounted(() => {
  carregarDados()
})

watch(() => $q.dark.isActive, atualizarGraficos)

function carregarGraficoPizza() {
  Highcharts.chart(graficoPizza.value, {
    chart: { type: 'pie' },
    title: { text: 'CADASTROS NO SISTEMA' },
    subtitle: { text: 'Distribuição geral' },
    tooltip: {
      pointFormat: '<b>{point.name}</b>: {point.y} ({point.percentage:.1f}%)'
    },
    plotOptions: {
      pie: {
        allowPointSelect: true,
        dataLabels: {
          enabled: true,
          format: '{point.name}: {point.y} ({point.percentage:.1f}%)'
        }
      }
    },
    series: [{
      name: 'Cadastros',
      colorByPoint: true,
      data: [
        { name: 'Clientes', y: dados.value.clientes },
        { name: 'Produtos', y: dados.value.produtos },
        { name: 'Ordens de Serviço', y: dados.value.ordens_servico },
        { name: 'Tickets', y: dados.value.tickets },
        { name: 'Usuários', y: dados.value.usuarios }
      ]
    }],
    colors: ['#1976D2', '#26A69A', '#FFA000', '#E53935', '#8E24AA']
  })
}

function carregarGraficoColunas() {
  Highcharts.chart(graficoColunas.value, {
    chart: { type: 'column' },
    title: { text: 'Resumo de Registros' },
    xAxis: {
      categories: ['Clientes', 'Produtos', 'Ordens de Serviço', 'Tickets', 'Usuários']
    },
    yAxis: {
      title: { text: 'Quantidade' }
    },
    tooltip: { valueSuffix: ' registros' },
    series: [{
      name: 'Total',
      data: [
        dados.value.clientes,
        dados.value.produtos,
        dados.value.ordens_servico,
        dados.value.tickets,
        dados.value.usuarios
      ],
      color: '#1976D2'
    }]
  })
}

function carregarGraficoStatusProduto() {
  const produtosInativos = dados.value.produtos - dados.value.produtosAtivos

  Highcharts.chart(graficoStatusProduto.value, {
    chart: { type: 'pie' },
    title: { text: 'Status dos Produtos' },
    subtitle: { text: 'Ativos x Inativos' },
    tooltip: {
      pointFormat: '{point.name}: <b>{point.y}</b> ({point.percentage:.1f}%)'
    },
    plotOptions: {
      pie: {
        allowPointSelect: true,
        dataLabels: {
          enabled: true,
          format: '{point.name}: {point.y} ({point.percentage:.1f}%)'
        }
      }
    },
    series: [{
      name: 'Produtos',
      colorByPoint: true,
      data: [
        { name: 'Ativos', y: dados.value.produtosAtivos },
        { name: 'Inativos', y: produtosInativos }
      ]
    }],
    colors: ['#2E7D32', '#C62828']
  })
}

async function downloadExcel() {
  try {
    loading.value = true

    const response = await api.get('/Relatorio/excel', {
      responseType: 'blob'
    })

    const blob = new Blob([response.data], {
      type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
    })

    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = `Relatorio_OSControl_${new Date().toISOString().slice(0, 10)}.xlsx`
    link.click()

    window.URL.revokeObjectURL(url)

    $q.notify({
      type: 'positive',
      message: 'Relatório baixado com sucesso!'
    })
  } catch (error) {
    console.error('Erro ao baixar relatório:', error)
    $q.notify({
      type: 'negative',
      message: 'Erro ao baixar relatório'
    })
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.shadow-2 {
  box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1);
}

.grafico-container {
  height: 300px;
}
</style>