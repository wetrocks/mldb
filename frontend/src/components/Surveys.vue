<template>
  <div>
    <button v-on:click="loaddata">Refresh</button>
      <h1>Surveys</h1>
      Here are my surveys
    <div>
        <table>
            <tr>
                <th>Name</th>
                <th>Date</th>
                <th>Volunteer Count</th>
            </tr>
            <tr v-for="survey in info" :key="survey.id">
                <td> {{survey.coordinator}} </td>
                <td> {{survey.date}} </td>
                <td> {{survey.volunteerCount}} </td>
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
  name: 'Surveys',
  props: {

  },
  data () {
    return {
      info: null,
      err: null
    }
  },
  mounted () {
     // this.loaddata();
  },
  methods: {
    loaddata: function () {
      axios
          .get('http://localhost:5000/api/survey')
          .then(response => {
              console.log(response);
              this.info = response.data;}
              )
          .catch(error => {
            this.err = error;
          })
    }
  }
}
</script>