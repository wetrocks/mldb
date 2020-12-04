<template>
  <div>
      <button v-on:click="loaddata">Refresh</button>
      <h1>Sites</h1>
      <hr />
      <input v-model="name" placeholder="name">
      <p>Name is: {{ name }}</p>  
      <button v-on:click="savedata">Save</button>
      Here are my sites
    <div>
        <table>
            <tr>
                <th>Name</th>
            </tr>
            <tr v-for="site in info" :key="site.id">
                <td> {{site.named}} </td>
            </tr>
        </table>
    </div>
    <hr />
    <div>{{ info }}</div>
    <div>error: {{ err }}</div>
  </div>
</template>

<script>
import axios from "axios";

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
      this.loaddata();
  },
  methods: {
    loaddata: function () {
      axios
          .get('http://localhost:5000/api/site')
          .then(response => {
              console.log(response);
              this.info = response.data;}
              )
          .catch(error => {
            this.err = error;
          })
    },
    savedata: function() {
      let newSite = {
        named: this.name
      };

      console.log(newSite);

      axios
          .post('http://localhost:5000/api/site', newSite)
          .then(response => {
            console.log(response);
          }) 
          .catch(error => {
            this.err = error;
          })
    }
  }
}
</script>