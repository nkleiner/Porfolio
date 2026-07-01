<script lang="ts">
  import { onMount } from 'svelte';
  import { createClient } from "@connectrpc/connect"; 
  import { createGrpcWebTransport } from "@connectrpc/connect-web";
  
  import { PortfolioService } from "../../gen/portfolio_connect";
  import type { Project } from "../../gen/portfolio_pb";

  // Svelte 5 Runes: Explicitly marking these as reactive UI state
  let projects: Project[] = $state([]);
  let loading = $state(true);
  let error = $state("");

  const transport = createGrpcWebTransport({
    baseUrl: "https://localhost:7023",
  });

  const client = createClient(PortfolioService, transport);

  onMount(async () => {
    try {
      const response = await client.getProjects({});
      projects = response.projects;
    } catch (e: any) {
      error = e.message;
    } finally {
      loading = false;
    }
  });
</script>

<main style="font-family: sans-serif; max-width: 800px; margin: 0 auto; padding: 2rem;">
  <h1>My Developer Portfolio</h1>

  {#if loading}
    <p>Loading projects via gRPC-Web...</p>
  {:else if error}
    <div style="background: #ffebee; padding: 1rem; border-radius: 4px; color: #c62828;">
      <strong>Connection Error:</strong> {error}
      <p style="font-size: 0.9em; margin-bottom: 0;">Is your .NET backend running, and does the port match the baseUrl in this file?</p>
    </div>
  {:else if projects.length === 0}
    <p>No projects found. The backend returned an empty list!</p>
  {:else}
    <ul style="list-style: none; padding: 0;">
      {#each projects as project}
        <li style="border: 1px solid #ccc; padding: 1rem; margin-bottom: 1rem; border-radius: 8px;">
          <h2 style="margin-top: 0;">{project.title}</h2>
          <p>{project.description}</p>
          <a href={project.url} target="_blank" style="color: #007bff; text-decoration: none; font-weight: bold;">View Source -></a>
        </li>
      {/each}
    </ul>
  {/if}
</main>