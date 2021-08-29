<template>
   <div>
    <div>
        <label for="amount_1">Polystyrene</label>
        <input type="number" step="1" id="amount_1" placeholder="count">
    </div>
    <div>
        <label for="amount_2">Cutlery</label>
        <input type="number" step="1" id="amount_2" placeholder="count">
    </div>
    <div>
        <label for="amount_3">Bottles</label>
        <input type="number" step="1" id="amount_3" placeholder="count">
    </div>
  </div>
</template>

<script>


export default {
  name: 'Sites',
  props: {

  },
  data () {
    return {
      name: null,
      info: null,
      err: null
    }
  },
  mounted () {
     
  },
  methods: {
  
  }
}
</script>