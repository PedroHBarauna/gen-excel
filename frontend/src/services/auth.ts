import { api } from "./api";
import { setAccessToken } from "./token";

export async function login() {
  const { data } = await api.post<{ accessToken: string }>("/api/auth/login", {
    email: "admin@genexcel.com",
    password: "123",
  });

  setAccessToken(data.accessToken);
  return data.accessToken;
}
