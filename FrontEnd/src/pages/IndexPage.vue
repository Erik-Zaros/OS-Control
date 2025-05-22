<template>
  <q-page class="q-pa-md bg-grey-2 dark:bg-dark">

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
  </q-page>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue'
import Highcharts from 'highcharts'
import { useQuasar } from 'quasar'

const $q = useQuasar()
const graficoPizza = ref(null)
const graficoColunas = ref(null)
const graficoStatusProduto = ref(null)

const dados = {
  clientes: 120,
  produtos: 40,
  ordens_servico: 125,
  produto_ativo: 39,
  produto_inativo: 1
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

  carregarGraficoPizza(dados)
  carregarGraficoColunas(dados)
  carregarGraficoStatusProduto(dados)
}

onMounted(atualizarGraficos)
watch(() => $q.dark.isActive, atualizarGraficos)

function carregarGraficoPizza(dados) {
  Highcharts.chart(graficoPizza.value, {
    chart: { type: 'pie' },
    title: { text: 'CADASTRO NO SISTEMA' },
    subtitle: { text: 'Cliente X Produto X OS' },
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
        { name: 'Clientes', y: dados.clientes },
        { name: 'Produtos', y: dados.produtos },
        { name: 'OS', y: dados.ordens_servico }
      ]
    }],
    colors: ['#1976D2', '#26A69A', '#FFA000']
  })
}

function carregarGraficoColunas(dados) {
  Highcharts.chart(graficoColunas.value, {
    chart: { type: 'column' },
    title: { text: 'Resumo de Registros' },
    xAxis: {
      categories: ['Clientes', 'Produtos', 'Ordens de Serviço']
    },
    yAxis: {
      title: { text: 'Quantidade' }
    },
    tooltip: { valueSuffix: ' registros' },
    series: [{
      name: 'Total',
      data: [dados.clientes, dados.produtos, dados.ordens_servico],
      color: '#1976D2'
    }]
  })
}

function carregarGraficoStatusProduto(dados) {
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
        { name: 'Ativos', y: dados.produto_ativo },
        { name: 'Inativos', y: dados.produto_inativo }
      ]
    }],
    colors: ['#2E7D32', '#C62828']
  })
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