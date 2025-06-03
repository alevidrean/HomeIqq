/*import React, { useState, useEffect } from "react";
import {
  Box,
  Typography,
  CircularProgress,
  Stack,
  TextField,
  Button,
  Paper,
} from "@mui/material";
import LightbulbIcon from "@mui/icons-material/Lightbulb";
import api from "../services/api";
import { useNavigate } from "react-router-dom";
import { getUserFullNameFromToken } from "../utils/jwt";
 
interface CameraData {
  temperature: number;
  humidity: number;
  ledState?: boolean;
}
 
interface SmartHomePayload {
  datetime: string;
  camera1: CameraData;
  camera2: CameraData;
  lockState: string;
}
 
export default function DashboardUser() {
  const name = getUserFullNameFromToken();
 
  const [lightStatus, setLightStatus] = useState<
    "on" | "off" | "loading" | "error"
  >("off");
 
  const [temperature, setTemperature] = useState<number>(22);
  const [tempStatus, setTempStatus] = useState<
    "idle" | "loading" | "success" | "error"
  >("idle");
 
  const [payload, setPayload] = useState<SmartHomePayload | null>(null);
  const [loadingPayload, setLoadingPayload] = useState<boolean>(false);
  const [errorPayload, setErrorPayload] = useState<string | null>(null);
 
  const navigate = useNavigate();
 
  const getToken = () => localStorage.getItem("token");
 
  useEffect(() => {
    const fetchData = async () => {
      setLoadingPayload(true);
      setErrorPayload(null);
      try {
        const response = await api.get<SmartHomePayload>("/CurrentTemperature", {
          headers: { Authorization: `Bearer ${getToken()}` },
        });
 
        setPayload(response.data);
 
        if (response.data.camera1.ledState !== undefined) {
          setLightStatus(response.data.camera1.ledState ? "on" : "off");
        }
 
        setLoadingPayload(false);
      } catch (error) {
        console.error("Eroare la fetch temperaturi:", error);
        setErrorPayload("Nu am putut încărca temperaturile.");
        setLoadingPayload(false);
      }
    };
 
    fetchData();
    const interval = setInterval(fetchData, 10000); // corect la 10 secunde
 
    return () => clearInterval(interval);
  }, []);
 
  const toggleLight = async () => {
    setLightStatus("loading");
    try {
      const endpoint = lightStatus === "on" ? "off" : "on";
      await api.post(
        `/light/${endpoint}`,
        {},
        {
          headers: { Authorization: `Bearer ${getToken()}` },
        }
      );
      setLightStatus(endpoint === "on" ? "on" : "off");
 
      const eventType = endpoint === "on" ? "LightOn" : "LightOff";
      await api.post(
        `/eventlog`,
        { eventType, details: "camera1" },
        {
          headers: { Authorization: `Bearer ${getToken()}` },
        }
      );
    } catch (error) {
      console.error("Eroare la backend/ESP sau EventLog (light):", error);
      setLightStatus("error");
    }
  };
 
  const setTemperatureHandler = async () => {
    setTempStatus("loading");
    try {
      await api.post(
        `/temperature/set`,
        { temperature },
        {
          headers: { Authorization: `Bearer ${getToken()}` },
        }
      );
      setTempStatus("success");
 
      await api.post(
        `/eventlog`,
        {
          eventType: "SetTemperature",
          details: `Temperatura setată la ${temperature}°C`,
        },
        {
          headers: { Authorization: `Bearer ${getToken()}` },
        }
      );
    } catch (error) {
      console.error("Eroare la setTemperature sau EventLog:", error);
      setTempStatus("error");
    }
  };
 
  const logout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("role");
    localStorage.removeItem("name");
    navigate("/");
  };
 
  // Stilizare culori butoane și iconițe
  const getButtonColor = () => {
    switch (lightStatus) {
      case "on":
        return "#4caf50";
      case "off":
        return "#f44336";
      case "loading":
      case "error":
        return "#9e9e9e";
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
    <Box
      sx={{
        minHeight: "100vh",
        background: "linear-gradient(135deg, #e0f7fa, #ffffff)",
        p: 4,
      }}
    >
      <Paper
        elevation={4}
        sx={{
          maxWidth: 700,
          mx: "auto",
          p: 4,
          borderRadius: 3,
          backgroundColor: "#e0f2f1",
        }}
      >
        <Typography
          variant="h4"
          gutterBottom
          sx={{ color: "#004d40", fontWeight: "bold" }}
        >
          Bine ai venit acasă, {name ? name : ""}!
        </Typography>
        <Typography variant="subtitle1" sx={{ mb: 3, color: "#004d40" }}>
          Spune-mi și te ajut imediat.
        </Typography>
 
        <Stack direction="column" spacing={4} alignItems="center">
          {// Temperatură & Umiditate 
          }
          <Stack direction="column" spacing={1} alignItems="center" sx={{ width: "100%" }}>
            <Typography variant="h6" sx={{ color: "#00796b" }}>
              Temperatură și umiditate curente
            </Typography>
            {loadingPayload && <CircularProgress />}
            {errorPayload && (
              <Typography color="error" sx={{ mt: 1 }}>
                {errorPayload}
              </Typography>
            )}
            {!loadingPayload && payload && (
              <>
                <Typography>
                  Camera 1: {payload.camera1.temperature} °C, Umiditate:{" "}
                  {payload.camera1.humidity}%
                </Typography>
                <Typography>
                  Camera 2: {payload.camera2.temperature} °C, Umiditate:{" "}
                  {payload.camera2.humidity}%
                </Typography>
                <Typography>Stare încuietoare: {payload.lockState}</Typography>
                <Typography>Ultima actualizare: {payload.datetime}</Typography>
              </>
            )}
          </Stack>
 
          {// Control lumină 
          }
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
              sx={{
                backgroundColor: getButtonColor(),
                color: "white",
                padding: "12px 24px",
                fontSize: "16px",
                borderRadius: "8px",
                minWidth: "180px",
                ":hover": {
                  backgroundColor:
                    lightStatus === "on"
                      ? "#388e3c"
                      : lightStatus === "off"
                      ? "#d32f2f"
                      : undefined,
                },
              }}
            >
              {lightStatus === "loading" ? (
                <CircularProgress size={24} color="inherit" />
              ) : (
                renderLabel()
              )}
            </Button>
          </Stack>
 
          {// Setare temperatură 
          }
          <Stack direction="column" spacing={2} alignItems="center" sx={{ width: "100%" }}>
            <Typography variant="h6" sx={{ color: "#00796b" }}>
              Setează temperatura
            </Typography>
            <TextField
              label="Temperatura (°C)"
              type="number"
              inputProps={{ min: 5, max: 35 }}
              value={temperature}
              onChange={(e) => setTemperature(parseInt(e.target.value, 10))}
              sx={{ width: 200 }}
            />
            <Button
              onClick={setTemperatureHandler}
              disabled={tempStatus === "loading"}
              variant="contained"
              sx={{
                backgroundColor:
                  tempStatus === "loading" ? "#9e9e9e" : "#1976d2",
                color: "white",
                padding: "12px 24px",
                fontSize: "16px",
                borderRadius: "8px",
                minWidth: "180px",
                ":hover": {
                  backgroundColor: tempStatus === "loading" ? undefined : "#115293",
                },
              }}
            >
              {tempStatus === "loading" ? (
                <CircularProgress size={24} color="inherit" />
              ) : (
                `Setează ${temperature}°C`
              )}
            </Button>
            {tempStatus === "error" && (
              <Typography color="error" sx={{ mt: 1 }}>
                A apărut o eroare la setarea temperaturii
              </Typography>
            )}
            {tempStatus === "success" && (
              <Typography color="success.main" sx={{ mt: 1 }}>
                Temperatura a fost setată cu succes
              </Typography>
            )}
          </Stack>
 
          {// Logout 
          }
          <Button
            onClick={logout}
            variant="outlined"
            color="error"
            sx={{ minWidth: "180px" }}
          >
            Logout
          </Button>
        </Stack>
      </Paper>
    </Box>
  );
}*/

/*import React, { useState, useEffect } from "react";
import {
  AppBar,
  Toolbar,
  IconButton,
  Box,
  Typography,
  CircularProgress,
  Stack,
  TextField,
  Button,
  Paper,
} from "@mui/material";
import LightbulbIcon from "@mui/icons-material/Lightbulb";
import LogoutIcon from "@mui/icons-material/Logout";
import api from "../services/api";
import { useNavigate } from "react-router-dom";
import { getUserFullNameFromToken } from "../utils/jwt";
import Logo from "../assets/logoA.png";

interface CameraData {
  temperature: number;
  humidity: number;
  ledState?: boolean;
}

interface SmartHomePayload {
  datetime: string;
  camera1: CameraData;
  camera2: CameraData;
  lockState: string;
}

export default function DashboardUser() {
  const name = getUserFullNameFromToken();

  const [lightStatus, setLightStatus] = useState<
    "on" | "off" | "loading" | "error"
  >("off");

  const [temperature, setTemperature] = useState<number>(22);
  const [tempStatus, setTempStatus] = useState<
    "idle" | "loading" | "success" | "error"
  >("idle");

  const [payload, setPayload] = useState<SmartHomePayload | null>(null);
  const [loadingPayload, setLoadingPayload] = useState<boolean>(false);
  const [errorPayload, setErrorPayload] = useState<string | null>(null);

  const navigate = useNavigate();

  const getToken = () => localStorage.getItem("token");

  useEffect(() => {
    const fetchData = async () => {
      setLoadingPayload(true);
      setErrorPayload(null);
      try {
        const response = await api.get<SmartHomePayload>("/CurrentTemperature", {
          headers: { Authorization: `Bearer ${getToken()}` },
        });

        setPayload(response.data);

        if (response.data.camera1.ledState !== undefined) {
          setLightStatus(response.data.camera1.ledState ? "on" : "off");
        }

        setLoadingPayload(false);
      } catch (error) {
        console.error("Eroare la fetch temperaturi:", error);
        setErrorPayload("Nu am putut încărca temperaturile.");
        setLoadingPayload(false);
      }
    };

    fetchData();
    const interval = setInterval(fetchData, 10000);

    return () => clearInterval(interval);
  }, []);

  const toggleLight = async () => {
    setLightStatus("loading");
    try {
      const endpoint = lightStatus === "on" ? "off" : "on";
      await api.post(
        `/light/${endpoint}`,
        {},
        {
          headers: { Authorization: `Bearer ${getToken()}` },
        }
      );
      setLightStatus(endpoint === "on" ? "on" : "off");

      const eventType = endpoint === "on" ? "LightOn" : "LightOff";
      await api.post(
        `/eventlog`,
        { eventType, details: "camera1" },
        {
          headers: { Authorization: `Bearer ${getToken()}` },
        }
      );
    } catch (error) {
      console.error("Eroare la backend/ESP sau EventLog (light):", error);
      setLightStatus("error");
    }
  };

  const setTemperatureHandler = async () => {
    setTempStatus("loading");
    try {
      await api.post(
        `/temperature/set`,
        { temperature },
        {
          headers: { Authorization: `Bearer ${getToken()}` },
        }
      );
      setTempStatus("success");

      await api.post(
        `/eventlog`,
        {
          eventType: "SetTemperature",
          details: `Temperatura setată la ${temperature}°C`,
        },
        {
          headers: { Authorization: `Bearer ${getToken()}` },
        }
      );
    } catch (error) {
      console.error("Eroare la setTemperature sau EventLog:", error);
      setTempStatus("error");
    }
  };

  const logout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("role");
    localStorage.removeItem("name");
    navigate("/");
  };

  const getButtonColor = () => {
    switch (lightStatus) {
      case "on":
        return "#4caf50";
      case "off":
        return "#f44336";
      case "loading":
      case "error":
        return "#9e9e9e";
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
    <Box
      sx={{
        minHeight: "100vh",
        background: "linear-gradient(135deg, #e0f7fa, #ffffff)",
        p: 0,
      }}
    >
      {// Bara de sus cu logo și logout 
      }
      <AppBar position="static" sx={{ backgroundColor: "#80deea" }}>
        <Toolbar sx={{ display: "flex", justifyContent: "space-between" }}>
          <Box
            sx={{ display: "flex", alignItems: "center", cursor: "pointer" }}
            onClick={() => navigate("/")}
          >
            <img src={Logo} alt="Logo" style={{ height: 60, marginRight: 12 }} />
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

      <Paper
        elevation={4}
        sx={{
          maxWidth: 700,
          mx: "auto",
          mt: 4,
          p: 4,
          borderRadius: 3,
          backgroundColor: "#e0f2f1",
        }}
      >
        <Typography
          variant="h4"
          gutterBottom
          sx={{ color: "#004d40", fontWeight: "bold" }}
        >
          Bine ai venit acasă, {name ? name : ""}!
        </Typography>
        <Typography variant="subtitle1" sx={{ mb: 3, color: "#004d40" }}>
          Spune-mi și te ajut imediat.
        </Typography>

        <Stack direction="column" spacing={4} alignItems="center">
          {// Temperatură & Umiditate 
          }
          <Stack direction="column" spacing={1} alignItems="center" sx={{ width: "100%" }}>
            <Typography variant="h6" sx={{ color: "#00796b" }}>
              Temperatură și umiditate curente
            </Typography>
            {loadingPayload && <CircularProgress />}
            {errorPayload && (
              <Typography color="error" sx={{ mt: 1 }}>
                {errorPayload}
              </Typography>
            )}
            {!loadingPayload && payload && (
              <>
                <Typography>
                  Camera 1: {payload.camera1.temperature} °C, Umiditate:{" "}
                  {payload.camera1.humidity}%
                </Typography>
                <Typography>
                  Camera 2: {payload.camera2.temperature} °C, Umiditate:{" "}
                  {payload.camera2.humidity}%
                </Typography>
                <Typography>Stare încuietoare: {payload.lockState}</Typography>
                <Typography>Ultima actualizare: {payload.datetime}</Typography>
              </>
            )}
          </Stack>

          {// Control lumină 
          }
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
              sx={{
                backgroundColor: getButtonColor(),
                color: "white",
                padding: "12px 24px",
                fontSize: "16px",
                borderRadius: "8px",
                minWidth: "180px",
                ":hover": {
                  backgroundColor:
                    lightStatus === "on"
                      ? "#388e3c"
                      : lightStatus === "off"
                      ? "#d32f2f"
                      : undefined,
                },
              }}
            >
              {lightStatus === "loading" ? (
                <CircularProgress size={24} color="inherit" />
              ) : (
                renderLabel()
              )}
            </Button>
          </Stack>

          {// Setare temperatură 
          }
          <Stack direction="column" spacing={2} alignItems="center" sx={{ width: "100%" }}>
            <Typography variant="h6" sx={{ color: "#00796b" }}>
              Setează temperatura
            </Typography>
            <TextField
              label="Temperatura (°C)"
              type="number"
              inputProps={{ min: 5, max: 35 }}
              value={temperature}
              onChange={(e) => setTemperature(parseInt(e.target.value, 10))}
              sx={{ width: 200 }}
            />
            <Button
              onClick={setTemperatureHandler}
              disabled={tempStatus === "loading"}
              variant="contained"
              sx={{
                backgroundColor:
                  tempStatus === "loading" ? "#9e9e9e" : "#1976d2",
                color: "white",
                padding: "12px 24px",
                fontSize: "16px",
                borderRadius: "8px",
                minWidth: "180px",
                ":hover": {
                  backgroundColor: tempStatus === "loading" ? undefined : "#115293",
                },
              }}
            >
              {tempStatus === "loading" ? (
                <CircularProgress size={24} color="inherit" />
              ) : (
                `Setează ${temperature}°C`
              )}
            </Button>
            {tempStatus === "error" && (
              <Typography color="error" sx={{ mt: 1 }}>
                A apărut o eroare la setarea temperaturii
              </Typography>
            )}
            {tempStatus === "success" && (
              <Typography color="success.main" sx={{ mt: 1 }}>
                Temperatura a fost setată cu succes
              </Typography>
            )}
          </Stack>
        </Stack>
      </Paper>
    </Box>
  );
}*/
import React, { useState, useEffect } from "react";
import {
  AppBar,
  Toolbar,
  IconButton,
  Box,
  Typography,
  CircularProgress,
  Stack,
  TextField,
  Button,
  Paper,
} from "@mui/material";
import LightbulbIcon from "@mui/icons-material/Lightbulb";
import LogoutIcon from "@mui/icons-material/Logout";
import api from "../services/api";
import { useNavigate } from "react-router-dom";
import { getUserFullNameFromToken } from "../utils/jwt";
import Logo from "../assets/logoA.png";

interface CameraData {
  temperature: number;
  humidity: number;
  ledState?: boolean;
}

interface SmartHomePayload {
  datetime: string;
  camera1: CameraData;
  camera2: CameraData;
  lockState: string;
}

export default function DashboardUser() {
  const name = getUserFullNameFromToken();

  const [lightStatus, setLightStatus] = useState<"on" | "off" | "loading" | "error">("off");
  const [temperature, setTemperature] = useState<number>(22);
  const [tempStatus, setTempStatus] = useState<"idle" | "loading" | "success" | "error">("idle");

  const [payload, setPayload] = useState<SmartHomePayload | null>(null);
  const [loadingPayload, setLoadingPayload] = useState<boolean>(false);
  const [errorPayload, setErrorPayload] = useState<string | null>(null);

  const navigate = useNavigate();

  const getToken = () => localStorage.getItem("token");

  useEffect(() => {
    const fetchData = async () => {
      setLoadingPayload(true);
      setErrorPayload(null);
      try {
        const response = await api.get<SmartHomePayload>("/CurrentTemperature", {
          headers: { Authorization: `Bearer ${getToken()}` },
        });

        setPayload(response.data);

        if (response.data.camera1.ledState !== undefined) {
          setLightStatus(response.data.camera1.ledState ? "on" : "off");
        }

        setLoadingPayload(false);
      } catch (error) {
        console.error("Eroare la fetch temperaturi:", error);
        setErrorPayload("Nu am putut încărca temperaturile.");
        setLoadingPayload(false);
      }
    };

    fetchData();
    const interval = setInterval(fetchData, 10000);

    return () => clearInterval(interval);
  }, []);

  const toggleLight = async () => {
    setLightStatus("loading");
    try {
      const endpoint = lightStatus === "on" ? "off" : "on";
      await api.post(`/light/${endpoint}`, {}, {
        headers: { Authorization: `Bearer ${getToken()}` },
      });
      setLightStatus(endpoint === "on" ? "on" : "off");

      const eventType = endpoint === "on" ? "LightOn" : "LightOff";
      await api.post(`/eventlog`, { eventType, details: "camera1" }, {
        headers: { Authorization: `Bearer ${getToken()}` },
      });
    } catch (error) {
      console.error("Eroare la backend/ESP sau EventLog (light):", error);
      setLightStatus("error");
    }
  };

  const setTemperatureHandler = async () => {
    setTempStatus("loading");
    try {
      await api.post(`/temperature/set`, { temperature }, {
        headers: { Authorization: `Bearer ${getToken()}` },
      });
      setTempStatus("success");

      await api.post(`/eventlog`, {
        eventType: "SetTemperature",
        details: `Temperatura setată la ${temperature}°C`,
      }, {
        headers: { Authorization: `Bearer ${getToken()}` },
      });
    } catch (error) {
      console.error("Eroare la setTemperature sau EventLog:", error);
      setTempStatus("error");
    }
  };

  const logout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("role");
    localStorage.removeItem("name");
    navigate("/");
  };

  const getButtonColor = () => {
    switch (lightStatus) {
      case "on":
        return "#4caf50";
      case "off":
        return "#f44336";
      case "loading":
      case "error":
        return "#9e9e9e";
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
    <Box
      sx={{
        minHeight: "100vh",
        background: "linear-gradient(135deg, #e0f7fa, #ffffff)",
        p: 0,
        display: "flex",
        flexDirection: "column",
      }}
    >
      <AppBar position="static" sx={{ backgroundColor: "#80deea" }}>
        <Toolbar sx={{ display: "flex", justifyContent: "space-between" }}>
          <Box
            sx={{ display: "flex", alignItems: "center", /*cursor: "pointer"*/}}
            //onClick={() => navigate("/")}
          >
            <img src={Logo} alt="Logo" style={{ height: 60, marginRight: 12 }} />
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

      <Paper
        elevation={4}
        sx={{
          maxWidth: 700,
          mx: "auto",
          mt: 4,
          p: 4,
          borderRadius: 3,
          backgroundColor: "#e0f2f1",
        }}
      >
        <Typography
          variant="h4"
          gutterBottom
          sx={{ color: "#004d40", fontWeight: "bold" }}
        >
          Bine ai venit acasă, {name ? name : ""}!
        </Typography>
        <Typography variant="subtitle1" sx={{ mb: 3, color: "#004d40" }}>
          Spune-mi și te ajut imediat.
        </Typography>

        <Stack direction="column" spacing={4} alignItems="center">
          <Stack direction="column" spacing={1} alignItems="center" sx={{ width: "100%" }}>
            <Typography variant="h6" sx={{ color: "#00796b" }}>
              Temperatură și umiditate curente
            </Typography>
            {loadingPayload && <CircularProgress />}
            {errorPayload && (
              <Typography color="error" sx={{ mt: 1 }}>
                {errorPayload}
              </Typography>
            )}
            {!loadingPayload && payload && (
              <>
                <Typography>
                  Camera 1: {payload.camera1.temperature} °C, Umiditate: {payload.camera1.humidity}%
                </Typography>
                <Typography>
                  Camera 2: {payload.camera2.temperature} °C, Umiditate: {payload.camera2.humidity}%
                </Typography>
                <Typography>Stare încuietoare: {payload.lockState}</Typography>
                <Typography>Ultima actualizare: {payload.datetime}</Typography>
              </>
            )}
          </Stack>

          <Stack direction="column" spacing={2} alignItems="center">
            <LightbulbIcon
              sx={{
                fontSize: 80,
                color: getBulbColor(),
                transition: "color 0.3s",
              }}
            />
            {/*<Button
              onClick={toggleLight}
              disabled={lightStatus === "loading"}
              sx={{
                backgroundColor: getButtonColor(),
                color: "white",
                padding: "12px 24px",
                fontSize: "16px",
                borderRadius: "8px",
                minWidth: "180px",
                ":hover": {
                  backgroundColor:
                    lightStatus === "on"
                      ? "#388e3c"
                      : lightStatus === "off"
                      ? "#d32f2f"
                      : undefined,
                },
              }}
            >
              {lightStatus === "loading" ? (
                <CircularProgress size={24} color="inherit" />
              ) : (
                renderLabel()
              )}
            </Button>*/}
            <Button
  onClick={toggleLight}
  disabled={lightStatus === "loading"}
  sx={{
    backgroundColor: getButtonColor(),
    color: "white",
    padding: "12px 24px",
    fontSize: "16px",
    borderRadius: "8px",
    minWidth: "180px",
    boxShadow: "0 6px 12px rgba(0,0,0,0.15)",
    transition: "background-color 0.3s ease, box-shadow 0.3s ease",
    ":hover": {
      backgroundColor:
        lightStatus === "on"
          ? "#388e3c"
          : lightStatus === "off"
          ? "#d32f2f"
          : undefined,
      boxShadow: "0 8px 20px rgba(0,0,0,0.3)",
    },
  }}
>
  {lightStatus === "loading" ? (
    <CircularProgress size={24} color="inherit" />
  ) : (
    renderLabel()
  )}
</Button>

          </Stack>

          <Stack direction="column" spacing={2} alignItems="center" sx={{ width: "100%" }}>
            <Typography variant="h6" sx={{ color: "#00796b" }}>
              Setează temperatura
            </Typography>
            <TextField
              label="Temperatura (°C)"
              type="number"
              inputProps={{ min: 5, max: 35 }}
              value={temperature}
              onChange={(e) => setTemperature(parseInt(e.target.value, 10))}
              sx={{ width: 200 }}
            />
            {/*<Button
              onClick={setTemperatureHandler}
              disabled={tempStatus === "loading"}
              variant="contained"
              sx={{
                backgroundColor: tempStatus === "loading" ? "#9e9e9e" : "#1976d2",
                color: "white",
                padding: "12px 24px",
                fontSize: "16px",
                borderRadius: "8px",
                minWidth: "180px",
                ":hover": {
                  backgroundColor: tempStatus === "loading" ? undefined : "#115293",
                },
              }}
            >
              {tempStatus === "loading" ? (
                <CircularProgress size={24} color="inherit" />
              ) : (
                `Setează ${temperature}°C`
              )}
            </Button>*/}
            <Button
  onClick={setTemperatureHandler}
  disabled={tempStatus === "loading"}
  variant="contained"
  sx={{
    backgroundColor: tempStatus === "loading" ? "#9e9e9e" : "#1976d2",
    color: "white",
    padding: "12px 24px",
    fontSize: "16px",
    borderRadius: "8px",
    minWidth: "180px",
    boxShadow: "0 6px 12px rgba(0,0,0,0.15)",
    transition: "background-color 0.3s ease, box-shadow 0.3s ease",
    ":hover": {
      backgroundColor: tempStatus === "loading" ? undefined : "#115293",
      boxShadow: tempStatus === "loading"
        ? undefined
        : "0 8px 20px rgba(0,0,0,0.3)",
    },
  }}
>
  {tempStatus === "loading" ? (
    <CircularProgress size={24} color="inherit" />
  ) : (
    `Setează ${temperature}°C`
  )}
</Button>

            {tempStatus === "error" && (
              <Typography color="error" sx={{ mt: 1 }}>
                A apărut o eroare la setarea temperaturii
              </Typography>
            )}
            {tempStatus === "success" && (
              <Typography color="success.main" sx={{ mt: 1 }}>
                Temperatura a fost setată cu succes
              </Typography>
            )}
          </Stack>
        </Stack>
      </Paper>

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










