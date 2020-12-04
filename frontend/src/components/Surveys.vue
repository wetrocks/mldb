<template>
  <div>
      <input v-model="coordinator" placeholder="coordinator">
      <input type="date" v-model="date" placeholder="date">
      <input type="number" step="1" v-model="count" placeholder="count">

      <input type="time" placeholder="Start Time">

      <button v-on:click="savedata">Save</button>
      <hr />
      <h1>Surveys</h1>
      <button v-on:click="loaddata">Refresh</button>
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
      err: null,
      coordinator: null,
      date: null,
      count: null,
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
    },
    savedata: function() {
      let newSurvey= {
        coordinator: this.coordinator,
        date: this.date,
        count: this.count,
      };

      console.log(newSurvey);

      axios
          .post('http://localhost:5000/api/survey', newSurvey)
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