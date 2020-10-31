<template>
  <div>
      <h1>Sites</h1>
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
      info: null,
      err: null
    }
  },
    mounted () {
    axios
      .get('http://localhost:5000/api/site')
      .then(response => {
          console.log(response);
          this.info = response.data;}
          )
      .catch(error => {
        this.err = error;
      })
  }
}
</script>