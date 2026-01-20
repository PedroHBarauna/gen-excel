import { createRoot } from "react-dom/client";
import App from "./App.tsx";
import "./index.css";
import { getAccessToken } from "./services/token.ts";
import { login } from "./services/auth.ts";
import React from "react";

async function bootstrap() {
  const token = getAccessToken?.() ?? localStorage.getItem("access_token");
  if (!token) {
    await login();
  }
}

bootstrap().finally(() => {
  createRoot(document.getElementById("root")!).render(
    <React.StrictMode>
      <App />
    </React.StrictMode>,
  );
});
