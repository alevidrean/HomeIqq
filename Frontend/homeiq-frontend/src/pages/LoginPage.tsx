// src/pages/LoginPage.tsx
/*import { useState } from "react";
import { Container, Paper, TextField, Typography, Button, Box } from "@mui/material";
import api from "../services/api";
import { useNavigate } from "react-router-dom";
interface LoginResponse {
  token: string;
  role: string;
}
export default function LoginPage() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      //const response = await api.post("/account/login", { username, password });
      //const { token, role } = response.data;
const response = await api.post<LoginResponse>("/account/login", { username, password });
const { token, role } = response.data;

      localStorage.setItem("token", token);
      localStorage.setItem("role", role);

      if (role === "Admin") navigate("/admin");
      else navigate("/user");
    } catch (err) {
      alert("Login eșuat");
    }
  };

  return (
    <Container maxWidth="sm">
      <Paper elevation={3} sx={{ p: 4, mt: 8 }}>
        <Typography variant="h5" gutterBottom align="center">
          Autentificare
        </Typography>
        <Box component="form" onSubmit={handleLogin}>
          <TextField
            fullWidth
            margin="normal"
            label="Username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
          />
          <TextField
            fullWidth
            margin="normal"
            label="Parolă"
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
          <Button
            fullWidth
            type="submit"
            variant="contained"
            sx={{ mt: 2 }}
          >
            Conectează-te
          </Button>
        </Box>
      </Paper>
    </Container>
  );
}*/

/*import { useState } from "react";
import {
  AppBar,
  Toolbar,
  Typography,
  Container,
  TextField,
  Button,
  Box,
  Paper,
} from "@mui/material";
import { useNavigate } from "react-router-dom";
import api from "../services/api";
import Logo from "../assets/logoA.png";
import { motion } from "framer-motion";

interface LoginResponse {
  token: string;
  role: string;
}

export default function LoginPage() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const response = await api.post<LoginResponse>("/account/login", {
        username,
        password,
      });
      const { token, role } = response.data;

      localStorage.setItem("token", token);
      localStorage.setItem("role", role);

      if (role === "Admin") navigate("/admin");
      else navigate("/user");
    } catch (err) {
      alert("Login eșuat");
    }
  };

  return (
    <Box
      sx={{
        minHeight: "100vh",
        background: "linear-gradient(135deg, #e0f7fa, #ffffff)",
      }}
    >
      {// AppBar identic cu Home 
      }
      <AppBar position="static" sx={{ backgroundColor: "#80deea" }}>
        <Toolbar>
          <Box sx={{ display: "flex", alignItems: "center", flexGrow: 1 }}>
            <img
  src={Logo}
  alt="Logo"
  style={{ height: 60, marginRight: 12, cursor: "pointer" }}
  onClick={() => navigate("/")} // ajustează ruta dacă e altă pagină
  />
          </Box>
        </Toolbar>
      </AppBar>

      {// Formular cu animație 
      }
      <Container maxWidth="sm">
        <motion.div
          initial={{ opacity: 0, scale: 0.9 }}
          animate={{ opacity: 1, scale: 1 }}
          transition={{ duration: 0.6 }}
        >
          <Paper
            elevation={4}
            sx={{
              p: 4,
              mt: 8,
              borderRadius: 3,
              backgroundColor: "#e0f2f1",
            }}
          >
            <Typography
              variant="h5"
              gutterBottom
              align="center"
              sx={{ color: "#004d40", fontWeight: "bold" }}
            >
              Autentificare
            </Typography>
            <Box component="form" onSubmit={handleLogin}>
              <TextField
                fullWidth
                margin="normal"
                label="Username"
                variant="outlined"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
              />
              <TextField
                fullWidth
                margin="normal"
                label="Parolă"
                type="password"
                variant="outlined"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />

              {// Buton animat 
              }
              <motion.div
                whileHover={{ scale: 1.05 }}
                whileTap={{ scale: 0.95 }}
                transition={{ type: "spring", stiffness: 300 }}
              >
                <Button
                  fullWidth
                  type="submit"
                  variant="contained"
                  sx={{
                    mt: 3,
                    backgroundColor: "#00796b",
                    ":hover": { backgroundColor: "#004d40" },
                    fontWeight: "bold",
                    py: 1.2,
                  }}
                >
                  Conectează-te
                </Button>
              </motion.div>
            </Box>
          </Paper>
        </motion.div>
      </Container>
    </Box>
  );
}*/

import { useState } from "react";
import {
  AppBar,
  Toolbar,
  Typography,
  Container,
  TextField,
  Button,
  Box,
  Paper,
} from "@mui/material";
import { useNavigate } from "react-router-dom";
import api from "../services/api";
import Logo from "../assets/logoA.png";
import { motion } from "framer-motion";
import { jwtDecode } from "jwt-decode"; // ✅ Import corect
 
interface LoginResponse {
  token: string;
  role: string;
}
 
interface JwtPayload {
  name: string;
  role: string;
  sub: string;
}
 
export default function LoginPage() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();
 
  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const response = await api.post<LoginResponse>("/account/login", {
        username,
        password,
      });
      const { token, role } = response.data;
 
      localStorage.setItem("token", token);
      localStorage.setItem("role", role);
 
      // ✅ Decodează tokenul și extrage numele
      const decoded = jwtDecode<JwtPayload>(token);
      localStorage.setItem("name", decoded.name);
 
      if (role === "Admin") navigate("/admin");
      else navigate("/user");
    } catch (err) {
      alert("Login eșuat");
    }
  };
 
  return (
    <Box
      sx={{
        minHeight: "100vh",
        background: "linear-gradient(135deg, #e0f7fa, #ffffff)",
      }}
    >
      <AppBar position="static" sx={{ backgroundColor: "#80deea" }}>
        <Toolbar>
          <Box sx={{ display: "flex", alignItems: "center", flexGrow: 1 }}>
            <img
              src={Logo}
              alt="Logo"
              style={{ height: 60, marginRight: 12, cursor: "pointer" }}
              onClick={() => navigate("/")}
            />
          </Box>
        </Toolbar>
      </AppBar>
 
      <Container maxWidth="sm">
        <motion.div
          initial={{ opacity: 0, scale: 0.9 }}
          animate={{ opacity: 1, scale: 1 }}
          transition={{ duration: 0.6 }}
        >
          <Paper
            elevation={4}
            sx={{
              p: 4,
              mt: 8,
              borderRadius: 3,
              backgroundColor: "#e0f2f1",
            }}
          >
            <Typography
              variant="h5"
              gutterBottom
              align="center"
              sx={{ color: "#004d40", fontWeight: "bold" }}
            >
              Autentificare
            </Typography>
            <Box component="form" onSubmit={handleLogin}>
              <TextField
                fullWidth
                margin="normal"
                label="Username"
                variant="outlined"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
              />
              <TextField
                fullWidth
                margin="normal"
                label="Parolă"
                type="password"
                variant="outlined"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
 
              <motion.div
                whileHover={{ scale: 1.05 }}
                whileTap={{ scale: 0.95 }}
                transition={{ type: "spring", stiffness: 300 }}
              >
                <Button
                  fullWidth
                  type="submit"
                  variant="contained"
                  sx={{
                    mt: 3,
                    backgroundColor: "#00796b",
                    ":hover": { backgroundColor: "#004d40" },
                    fontWeight: "bold",
                    py: 1.2,
                  }}
                >
                  Conectează-te
                </Button>
              </motion.div>
            </Box>
          </Paper>
        </motion.div>
      </Container>
      {/* Spațiu de final și footer */}
            <Box sx={{ height: 40 }} />
            <Typography
              variant="body2"
              align="center"
              color="text.secondary"
              sx={{ pb: 2 }}
            >
              HomeIQ © {new Date().getFullYear()}
            </Typography>
          </Box>
  );
}


