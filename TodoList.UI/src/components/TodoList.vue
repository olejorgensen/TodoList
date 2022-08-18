<template>
    <div class="post">
        <div v-if="loading" class="loading">
            Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationvue">https://aka.ms/jspsintegrationvue</a> for more details.
        </div>

        <div v-if="post" class="content">
            <table>
                <thead>
                    <tr>
                        <th>Done</th>
                        <th>Name</th>
                        <th>Description</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="todoItem in post" :key="todoItem.id">
                        <td>{{ todoItem.isDone }}</td>
                        <td>{{ todoItem.name }}</td>
                        <td>{{ todoItem.description }}</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';

    type TodoItems = {
        id: number,
        name: string,
        description: string,
        isDone: boolean
    }[];

    interface Data {
        loading: boolean,
        post: null | TodoItems
    }

    export default defineComponent({
        data(): Data {
            return {
                loading: false,
                post: null
            };
        },
        created() {
            // fetch the data when the view is created and the data is
            // already being observed
            this.fetchData();
        },
        watch: {
            // call again the method if the route changes
            '$route': 'fetchData'
        },
        methods: {
            fetchData(): void {
                this.post = null;
                this.loading = true;

                fetch('http://localhost:5150/TodoList', { mode: 'no-cors'})
                    .then(r => r.json())
                    .then(json => {
                        this.post = json as TodoItems;
                        this.loading = false;
                        return;
                    });
            }
        },
    });
</script>