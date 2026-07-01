<script lang="ts">
  import { createClient } from "@connectrpc/connect"; 
  import { createGrpcWebTransport } from "@connectrpc/connect-web";
  
  // Import the new Tavern service we just generated
  import { DNDService } from "../../../gen/dungeons_and_dragons_connect";

  // Svelte 5 State Runes
  let rollNumber: number | null = $state(null);
  let magicEffect: string = $state("");
  let isRolling: boolean = $state(false);
  let error: string = $state("");

  const transport = createGrpcWebTransport({
    baseUrl: "https://localhost:7023", // Ensure this matches your .NET port
  });

  const client = createClient(DNDService, transport);

  async function rollDice() {
    isRolling = true;
    error = "";
    rollNumber = null;
    magicEffect = "";

    try {
      // Add a tiny artificial delay just for the suspense/UI feel
      await new Promise(resolve => setTimeout(resolve, 600)); 
      
      const response = await client.getWildMagicRoll({});
      magicEffect = response.wildMagicEffect;
      rollNumber = response.roll;
    } catch (e: any) {
      error = e.message;
    } finally {
      isRolling = false;
    }
  }
</script>

<div class="tavern-container">
  <div class="content-wrapper">
    <h1>🐉 The Hidden Tavern</h1>
    <p class="subtitle">DM Tool: Extended Wild Magic Surge</p>

    <div class="control-panel">
      <button onclick={rollDice} disabled={isRolling} class="roll-button">
        {isRolling ? "Rolling d100..." : "Roll for Chaos"}
      </button>
    </div>

    {#if error}
      <div class="error-box">
        <strong>Connection Failed:</strong> {error}
        <p>Is the C# TavernService running?</p>
      </div>
    {/if}

    {#if rollNumber !== null}
      <div class="result-box">
        <div class="dice-result">
          <span class="label">Result:</span>
          <span class="number">{rollNumber}</span>
        </div>
        <p class="effect-text">{magicEffect}</p>
      </div>
    {/if}
  </div>
</div>

<style>
  /* Scoped CSS to keep the Tavern vibes completely isolated from your portfolio */
  .tavern-container {
    background-color: #1a1a1d;
    color: #e0e0e0;
    min-height: 100vh;
    padding: 3rem 1rem;
    font-family: 'Courier New', Courier, monospace;
    display: flex;
    justify-content: center;
  }

  .content-wrapper {
    max-width: 600px;
    width: 100%;
  }

  h1 {
    color: #d4af37; /* Metallic Gold */
    margin-bottom: 0.2rem;
  }

  .subtitle {
    color: #888;
    margin-bottom: 2rem;
    font-size: 0.9rem;
  }

  .control-panel {
    margin-bottom: 2rem;
  }

  .roll-button {
    background-color: #4a0e17; /* Deep Crimson */
    color: white;
    border: 2px solid #d4af37;
    padding: 1rem 2rem;
    font-size: 1.2rem;
    font-family: inherit;
    font-weight: bold;
    cursor: pointer;
    border-radius: 8px;
    transition: all 0.2s ease;
    width: 100%;
  }

  .roll-button:hover:not(:disabled) {
    background-color: #7a1625;
    box-shadow: 0 0 15px rgba(212, 175, 55, 0.4);
  }

  .roll-button:disabled {
    background-color: #2a2a2d;
    border-color: #555;
    color: #888;
    cursor: not-allowed;
  }

  .result-box {
    background-color: #242428;
    border: 1px solid #d4af37;
    border-radius: 8px;
    padding: 2rem;
    animation: fadeIn 0.4s ease-out;
  }

  .dice-result {
    display: flex;
    align-items: center;
    gap: 1rem;
    margin-bottom: 1rem;
    border-bottom: 1px solid #444;
    padding-bottom: 1rem;
  }

  .number {
    font-size: 2.5rem;
    color: #d4af37;
    font-weight: bold;
  }

  .effect-text {
    font-size: 1.1rem;
    line-height: 1.6;
  }

  .error-box {
    background: #4a0e17;
    border: 1px solid #ff4444;
    padding: 1rem;
    border-radius: 8px;
  }

  @keyframes fadeIn {
    from { opacity: 0; transform: translateY(10px); }
    to { opacity: 1; transform: translateY(0); }
  }
</style>