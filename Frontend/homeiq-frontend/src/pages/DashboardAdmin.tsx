/*import { useState } from "react";
import {
  Button,
  Typography,
  CircularProgress,
  Stack,
} from "@mui/material";
import LightbulbIcon from "@mui/icons-material/Lightbulb";
import axios from "axios";
import { useNavigate } from "react-router-dom";

interface ToggleLightResponse { //linie noua
  status: "on" | "off";         //linie noua
}
export default function DashboardAdmin() {
  const [lightStatus, setLightStatus] = useState<"on" | "off" | "loading" | "error">("off");
const navigate = useNavigate();

  const toggleLight = async () => {
    setLightStatus("loading");
    try {
      //const response = await axios.post("http://localhost:5033/api/device/toggle-light");
      //const newStatus = response.data.status === "on" ? "on" : "off";
      const response = await axios.post<ToggleLightResponse>("http://localhost:5033/api/device/toggle-light");
      const newStatus = response.data.status === "on" ? "on" : "off";

      setLightStatus(newStatus);
    } catch (error) {
      setLightStatus("error");
      console.error("Eroare la backend/ESP:", error);
    }
  };

    const logout = () => {
    localStorage.removeItem("token");  // Șterge tokenul
    navigate("/");                     // Redirect către homepage
  };

  const getButtonColor = () => {
    switch (lightStatus) {
      case "on":
        return "#4caf50"; // verde
      case "off":
        return "#f44336"; // roșu
      case "error":
        return "#9e9e9e"; // gri
      case "loading":
        return "#9e9e9e"; // gri
      default:
        return "#9e9e9e";
    }
  };

  const renderLabel = () => {
    if (lightStatus === "loading") return "Se schimbă...";
    if (lightStatus === "on") return "Stinge becul";
    if (lightStatus === "off") return "Aprinde becul";
    return "Eroare la ESP";
  };

  const getBulbColor = () => {
    if (lightStatus === "on") return "#ffeb3b"; // galben aprins
    if (lightStatus === "loading") return "#bdbdbd"; // gri
    return "#9e9e9e"; // gri stins
  };

  return (
    <div style={{ padding: 20 }}>
      <Typography variant="h4" gutterBottom>
        Dashboard Admin
      </Typography>

      <Stack direction="column" spacing={2} alignItems="center">
        <LightbulbIcon
          sx={{
            fontSize: 80,
            color: getBulbColor(),
            transition: "color 0.3s",
          }}
        />

        <Button
          onClick={toggleLight}
          disabled={lightStatus === "loading"}
          style={{
            backgroundColor: getButtonColor(),
            color: "white",
            padding: "12px 24px",
            fontSize: "16px",
            borderRadius: "8px",
            minWidth: "180px",
          }}
        >
          {lightStatus === "loading" ? (
            <CircularProgress size={24} color="inherit" />
          ) : (
            renderLabel()
          )}
        </Button>
        <Button
            onClick={() => navigate("/admin/create-user")}
            variant="contained"
           style={{ backgroundColor: "#1976d2", color: "white" }}
         >
          Creează utilizator nou
         </Button>
            <Button
          onClick={logout}
          variant="outlined"
          color="error"
          style={{ minWidth: "180px" }}
        >
          Logout
        </Button>
      </Stack>
    </div>
  );
}*/
import { useState } from "react";
import {
  AppBar,
  Toolbar,
  Button,
  Typography,
  CircularProgress,
  Stack,
  Box,
  Paper,
} from "@mui/material";
import LightbulbIcon from "@mui/icons-material/Lightbulb";
import LogoutIcon from "@mui/icons-material/Logout";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import logo from "../assets/logoA.png";

interface ToggleLightResponse {
  status: "on" | "off";
}

export default function DashboardAdmin() {
  const [lightStatus, setLightStatus] = useState<"on" | "off" | "loading" | "error">("off");
  const navigate = useNavigate();

  const toggleLight = async () => {
    setLightStatus("loading");
    try {
      const response = await axios.post<ToggleLightResponse>(
        "http://localhost:5033/api/device/toggle-light"
      );
      const newStatus = response.data.status === "on" ? "on" : "off";
      setLightStatus(newStatus);
    } catch (error) {
      setLightStatus("error");
      console.error("Eroare la backend/ESP:", error);
    }
  };

  const logout = () => {
    localStorage.removeItem("token");
    navigate("/");
  };

  const getButtonColor = () => {
    switch (lightStatus) {
      case "on":
        return "#4caf50";
      case "off":
        return "#f44336";
      case "error":
      case "loading":
      default:
        return "#9e9e9e";
    }
  };

  const renderLabel = () => {
    if (lightStatus === "loading") return "Se schimbă...";
    if (lightStatus === "on") return "Stinge becul";
    if (lightStatus === "off") return "Aprinde becul";
    return "Eroare la ESP";
  };

  const getBulbColor = () => {
    if (lightStatus === "on") return "#ffeb3b";
    if (lightStatus === "loading") return "#bdbdbd";
    return "#9e9e9e";
  };

  return (
    <Box sx={{ minHeight: "100vh", background: "linear-gradient(135deg, #e0f7fa, #ffffff)" }}>
      {/* AppBar */}
      <AppBar position="static" sx={{ backgroundColor: "#80deea" }}>
        <Toolbar sx={{ display: "flex", justifyContent: "space-between" }}>
          <Box sx={{ display: "flex", alignItems: "center" }}>
            <img
  src={logo}
  alt="Logo"
  style={{ height: 60, marginRight: 12, /*cursor: "pointer"*/ }}
  //onClick={() => navigate("/")} // ajustează ruta dacă e altă pagină
  />
          </Box>
          <Button
            onClick={logout}
            variant="outlined"
            color="error"
            startIcon={<LogoutIcon />}
            sx={{
              fontWeight: "bold",
              textTransform: "uppercase",
              letterSpacing: 1,
              borderWidth: 2,
              "&:hover": {
                borderWidth: 2,
                backgroundColor: "#f44336",
                color: "white",
              },
            }}
          >
            Logout
          </Button>
        </Toolbar>
      </AppBar>

      {/* Conținut principal */}
      <Box sx={{ display: "flex", justifyContent: "center", p: 4 }}>
        <Paper
          elevation={8}
          sx={{
            maxWidth: 700,
            width: "100%",
            borderRadius: 4,
            p: 5,
            mt: 4,
            backgroundColor: "rgba(255,255,255,0.95)",
            boxShadow: "0 8px 16px rgba(0,0,0,0.15), 0 4px 8px rgba(0,0,0,0.1)",
          }}
        >
          <Box sx={{ textAlign: "center", mb: 4 }}>
            <Typography variant="h4" sx={{ fontWeight: "700", color: "#00796b" }}>
              Bine ai venit, admin!
            </Typography>
            <Typography variant="subtitle1" sx={{ color: "#004d40" }}>
              Controlează luminile și adaugă utilizatori noi!
            </Typography>
          </Box>

          <Stack spacing={4} alignItems="center">
            <LightbulbIcon
              sx={{
                fontSize: 100,
                color: getBulbColor(),
                filter: lightStatus === "on" ? "drop-shadow(0 0 8px #ffeb3b)" : "none",
                transition: "all 0.4s ease",
                animation: lightStatus === "loading" ? "pulse 1.5s infinite" : "none",
                cursor: lightStatus === "loading" ? "wait" : "pointer",
              }}
            />
            <Button
              onClick={toggleLight}
              disabled={lightStatus === "loading"}
              sx={{
                backgroundColor: getButtonColor(),
                color: "white",
                padding: "14px 28px",
                fontSize: "18px",
                borderRadius: "12px",
                fontWeight: "600",
                boxShadow: "0 6px 12px rgba(0,0,0,0.15)",
                transition: "background-color 0.3s ease",
                "&:hover": {
                  backgroundColor: lightStatus === "on" ? "#388e3c" : "#d32f2f",
                  boxShadow: "0 8px 20px rgba(0,0,0,0.3)",
                },
                minWidth: "200px",
              }}
            >
              {lightStatus === "loading" ? (
                <CircularProgress size={26} color="inherit" />
              ) : (
                renderLabel()
              )}
            </Button>

            <Button
              variant="contained"
              onClick={() => navigate("/admin/create-user")}
              sx={{
                backgroundColor: "#0288d1",
                color: "white",
                fontWeight: "600",
                padding: "12px 28px",
                borderRadius: "12px",
                boxShadow: "0 5px 15px rgba(2,136,209,0.4)",
                transition: "background-color 0.3s ease",
                "&:hover": {
                  backgroundColor: "#0277bd",
                  boxShadow: "0 8px 20px rgba(2,119,189,0.6)",
                },
                minWidth: "200px",
              }}
            >
              Creează utilizator nou
            </Button>
          </Stack>
        </Paper>
      </Box>
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



