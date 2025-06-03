/*import { useState } from "react";
import {
  TextField,
  Button,
  Typography,
  Container,
  Stack,
  Alert,
} from "@mui/material";
import axios from "axios";

export default function CreateUser() {
  const [form, setForm] = useState({
    username: "",
    email: "",
    nume: "",
    prenume: "",
    cnp: "",
    codBluetooth: "",
    password: "",
  });
  const [success, setSuccess] = useState("");
  const [error, setError] = useState("");

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async () => {
    setError("");
    setSuccess("");
    try {
      const token = localStorage.getItem("token");
      await axios.post("http://localhost:5033/api/users/create", form, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      setSuccess("Utilizator creat cu succes.");
      setForm({
        username: "",
        email: "",
        nume: "",
        prenume: "",
        cnp: "",
        codBluetooth: "",
        password: "",
      });
    } catch (err: any) {
      setError(err.response?.data || "Eroare la creare.");
    }
  };

  return (
    <Container maxWidth="sm" sx={{ mt: 5 }}>
      <Typography variant="h5" gutterBottom>
        Creare utilizator nou
      </Typography>
      <Stack spacing={2}>
        <TextField label="Username" name="username" value={form.username} onChange={handleChange} />
        <TextField label="Email" name="email" value={form.email} onChange={handleChange} />
        <TextField label="Nume" name="nume" value={form.nume} onChange={handleChange} />
        <TextField label="Prenume" name="prenume" value={form.prenume} onChange={handleChange} />
        <TextField label="CNP" name="cnp" value={form.cnp} onChange={handleChange} />
        <TextField label="Cod Bluetooth" name="codBluetooth" value={form.codBluetooth} onChange={handleChange} />
        <TextField label="Parolă" type="password" name="password" value={form.password} onChange={handleChange} />
        <Button variant="contained" onClick={handleSubmit}>
          Creează
        </Button>
        {success && <Alert severity="success">{success}</Alert>}
        {error && <Alert severity="error">{error}</Alert>}
      </Stack>
    </Container>
  );
}*/

/*import { useState } from "react";
import {
  AppBar,
  Toolbar,
  Typography,
  Container,
  Stack,
  Alert,
  Paper,
  Box,
  TextField,
  Button,
} from "@mui/material";
import axios from "axios";
import logo from "../assets/logoA.png";
import { useNavigate } from "react-router-dom"; // asigură-te că e importat
export default function CreateUser() {
  const [form, setForm] = useState({
    username: "",
    email: "",
    nume: "",
    prenume: "",
    cnp: "",
    codBluetooth: "",
    password: "",
  });
  const [success, setSuccess] = useState("");
  const [error, setError] = useState("");

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };
const navigate = useNavigate(); // în componentă
  const handleSubmit = async () => {
    setSuccess("");
    setError("");
    try {
      const token = localStorage.getItem("token");
      await axios.post("http://localhost:5033/api/users/create", form, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      setSuccess("Utilizator creat cu succes.");
      setForm({
        username: "",
        email: "",
        nume: "",
        prenume: "",
        cnp: "",
        codBluetooth: "",
        password: "",
      });
    } catch (err: any) {
      setError(err.response?.data || "Eroare la creare.");
    }
  };

  return (
    <Box sx={{ minHeight: "100vh", background: "linear-gradient(135deg, #e0f7fa, #ffffff)" }}>
      {// AppBar cu logo, fără logout
      }
      <AppBar position="static" sx={{ backgroundColor: "#80deea" }}>
        <Toolbar>
          <Box sx={{ display: "flex", alignItems: "center" }}>
    <img
  src={logo}
  alt="Logo"
  style={{ height: 60, marginRight: 12, cursor: "pointer" }}
  onClick={() => navigate("/")} // ajustează ruta dacă e altă pagină
/>
          </Box>
        </Toolbar>
      </AppBar>

      { //Conținut principal 
       }
      <Container maxWidth="sm" sx={{ py: 6 }}>
        <Paper
          elevation={8}
          sx={{
            p: 5,
            borderRadius: 4,
            backgroundColor: "rgba(255,255,255,0.95)",
            boxShadow: "0 8px 16px rgba(0,0,0,0.15), 0 4px 8px rgba(0,0,0,0.1)",
          }}
        >
          <Typography variant="h4" align="center" sx={{ fontWeight: "700", color: "#00796b", mb: 2 }}>
            Creare utilizator nou
          </Typography>

          <Typography variant="subtitle1" align="center" sx={{ color: "#004d40", mb: 3 }}>
            Completează datele pentru a crea un nou cont.
          </Typography>

          <Stack spacing={2}>
            <TextField label="Username" name="username" value={form.username} onChange={handleChange} fullWidth />
            <TextField label="Email" name="email" value={form.email} onChange={handleChange} fullWidth />
            <TextField label="Nume" name="nume" value={form.nume} onChange={handleChange} fullWidth />
            <TextField label="Prenume" name="prenume" value={form.prenume} onChange={handleChange} fullWidth />
            <TextField label="CNP" name="cnp" value={form.cnp} onChange={handleChange} fullWidth />
            <TextField label="Cod Bluetooth" name="codBluetooth" value={form.codBluetooth} onChange={handleChange} fullWidth />
            <TextField label="Parolă" type="password" name="password" value={form.password} onChange={handleChange} fullWidth />

            <Button
              variant="contained"
              onClick={handleSubmit}
              sx={{
                backgroundColor: "#00796b",
                color: "white",
                fontWeight: "600",
                padding: "12px 28px",
                borderRadius: "12px",
                boxShadow: "0 5px 15px rgba(2,136,209,0.4)",
                transition: "background-color 0.3s ease",
                "&:hover": {
                  backgroundColor: "#004d40",
                  boxShadow: "0 8px 20px rgba(2,119,189,0.6)",
                },
              }}
            >
              Creează
            </Button>

            {success && <Alert severity="success">{success}</Alert>}
            {error && <Alert severity="error">{error}</Alert>}
          </Stack>
        </Paper>
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
  Stack,
  Alert,
  Paper,
  Box,
  TextField,
  Button,
} from "@mui/material";
import axios from "axios";
import logo from "../assets/logoA.png";
import { useNavigate } from "react-router-dom";
import { motion } from "framer-motion";

export default function CreateUser() {
  const [form, setForm] = useState({
    username: "",
    email: "",
    nume: "",
    prenume: "",
    cnp: "",
    codBluetooth: "",
    password: "",
  });
  const [success, setSuccess] = useState("");
  const [error, setError] = useState("");

  const navigate = useNavigate();

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async () => {
    setSuccess("");
    setError("");
    try {
      const token = localStorage.getItem("token");
      await axios.post("http://localhost:5033/api/users/create", form, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      setSuccess("Utilizator creat cu succes.");
      setForm({
        username: "",
        email: "",
        nume: "",
        prenume: "",
        cnp: "",
        codBluetooth: "",
        password: "",
      });
    } catch (err: any) {
      setError(err.response?.data || "Eroare la creare.");
    }
  };

  return (
    <Box sx={{ minHeight: "100vh", background: "linear-gradient(135deg, #e0f7fa, #ffffff)" }}>
      {/* AppBar identic cu Login */}
      <AppBar position="static" sx={{ backgroundColor: "#80deea" }}>
        <Toolbar>
          <Box sx={{ display: "flex", alignItems: "center", flexGrow: 1 }}>
            <img
              src={logo}
              alt="Logo"
              style={{ height: 60, marginRight: 12, cursor: "pointer" }}
              onClick={() => navigate("/")}
            />
          </Box>
        </Toolbar>
      </AppBar>

      {/* Form container cu animație */}
      <Container maxWidth="sm">
        <motion.div
          initial={{ opacity: 0, scale: 0.95 }}
          animate={{ opacity: 1, scale: 1 }}
          transition={{ duration: 0.5 }}
        >
          <Paper
            elevation={6}
            sx={{
              p: 5,
              mt: 6,
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
              Creare utilizator nou
            </Typography>

            <Stack spacing={2}>
              <TextField label="Username" name="username" value={form.username} onChange={handleChange} fullWidth />
              <TextField label="Email" name="email" value={form.email} onChange={handleChange} fullWidth />
              <TextField label="Nume" name="nume" value={form.nume} onChange={handleChange} fullWidth />
              <TextField label="Prenume" name="prenume" value={form.prenume} onChange={handleChange} fullWidth />
              <TextField label="CNP" name="cnp" value={form.cnp} onChange={handleChange} fullWidth />
              <TextField label="Cod Bluetooth" name="codBluetooth" value={form.codBluetooth} onChange={handleChange} fullWidth />
              <TextField label="Parolă" type="password" name="password" value={form.password} onChange={handleChange} fullWidth />

              <motion.div
                whileHover={{ scale: 1.05 }}
                whileTap={{ scale: 0.95 }}
                transition={{ type: "spring", stiffness: 300 }}
              >
                <Button
                  variant="contained"
                  onClick={handleSubmit}
                  fullWidth
                  sx={{
                    mt: 1,
                    backgroundColor: "#00796b",
                    ":hover": { backgroundColor: "#004d40" },
                    fontWeight: "bold",
                    py: 1.2,
                  }}
                >
                  Creează
                </Button>
              </motion.div>

              {success && <Alert severity="success">{success}</Alert>}
              {error && <Alert severity="error">{error}</Alert>}
            </Stack>
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


