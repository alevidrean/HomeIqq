import { jwtDecode } from "jwt-decode";

interface JwtPayload {
  firstName: string;
  lastName: string;
  role: string;
  sub: string;
}

export const getUserFullNameFromToken = (): string | null => {
  const token = localStorage.getItem("token");
  if (!token) return null;

  try {
    const decoded = jwtDecode<JwtPayload>(token);
    return ` ${decoded.lastName} ${decoded.firstName}`;
  } catch {
    return null;
  }
};