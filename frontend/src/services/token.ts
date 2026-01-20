const TOKEN_KEY = "accessToken";

let cachedToken: string | null = localStorage.getItem(TOKEN_KEY);

export function getAccessToken() {
  return cachedToken;
}

export function setAccessToken(token: string | null) {
  cachedToken = token;
  if (token) localStorage.setItem(TOKEN_KEY, token);
  else localStorage.removeItem(TOKEN_KEY);
}

export function clearAccessToken() {
  setAccessToken(null);
}
