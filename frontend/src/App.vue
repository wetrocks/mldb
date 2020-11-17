<template>
 
  <div>
    <Sites />

    <hr>

    <Surveys />
  </div>
 

</template>

<script>
import Sites from './components/Sites.vue'
import Surveys from './components/Surveys.vue'
//import HelloWorld from './components/HelloWorld.vue'

export default {
  name: 'App',
  components: {
   // HelloWorld,
    Sites,
    Surveys
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
