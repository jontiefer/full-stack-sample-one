/// <reference types="vite/client" />

interface ImportMetaEnv {
    // Allows any property name starting with VITE_ and its value will be string or undefined
    [key: `VITE_${string}`]: string | undefined;
  }
  
  interface ImportMeta {
    readonly env: ImportMetaEnv;
  }