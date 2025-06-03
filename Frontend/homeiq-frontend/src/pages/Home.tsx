/*import { Button, Container, Typography, Stack } from "@mui/material";
import { useNavigate } from "react-router-dom";

export default function Home() {
  const navigate = useNavigate();

  return (
    <Container sx={{ mt: 8, textAlign: "center" }}>
      <Typography variant="h3" gutterBottom>
        Bine ai venit la Home Automation
      </Typography>

      <Typography variant="body1" sx={{ mb: 4 }}>
        Controlează luminile, temperatura și mai multe dintr-un singur loc.
      </Typography>

      <Stack direction="row" spacing={2} justifyContent="center">
        <Button variant="contained" onClick={() => navigate("/login")}>
          Conectare
        </Button>
      </Stack>
    </Container>
  );
}*/

/*import {
  AppBar,
  Toolbar,
  Typography,
  Container,
  Button,
  Stack,
  Box
} from "@mui/material";
import { useNavigate } from "react-router-dom";
import Logo from "../assets/logoA.png"; // modifică dacă ai alt path

export default function Home() {
  const navigate = useNavigate();

  return (
    <>
      <AppBar position="static" sx={{ backgroundColor: "#80deea" }}>
        <Toolbar>
          <Box sx={{ display: "flex", alignItems: "center", flexGrow: 1 }}>
            <img src={Logo} alt="Logo" style={{ height: 70, marginRight: 12 }} />
          </Box>
        </Toolbar>
      </AppBar>

      <Container sx={{ mt: 10, textAlign: "center" }}>
        <Typography variant="h3" gutterBottom>
          Bine ai venit la Home IQ!
        </Typography>

        <Typography variant="body1" sx={{ mb: 4 }}>
          Controlează luminile, temperatura și mai multe dintr-un singur loc.
        </Typography>

        <Stack direction="row" spacing={2} justifyContent="center">
          <Button variant="contained" size="large" onClick={() => navigate("/login")}>
            Conectare
          </Button>
        </Stack>
      </Container>
    </>
  );
}*/
import {
  AppBar,
  Toolbar,
  Typography,
  Container,
  Button,
  Stack,
  Box,
} from "@mui/material";
import { useNavigate } from "react-router-dom";
import { motion } from "framer-motion";
import Logo from "../assets/logoA.png";
import SmartHomeImg from "../assets/smart-home.png"; // ai salvat imaginea generată ca acest fișier

export default function Home() {
  const navigate = useNavigate();

  return (
    <Box
      sx={{
        minHeight: "100vh",
        background: "linear-gradient(135deg, #e0f7fa, #ffffff)",
      }}
    >
      {/* Bara de sus */}
      <AppBar position="static" sx={{ backgroundColor: "#80deea" }}>
        <Toolbar>
          <Box sx={{ display: "flex", alignItems: "center", flexGrow: 1 }}>
            <img src={Logo} alt="Logo" style={{ height: 60, marginRight: 12 }} />
          </Box>
        </Toolbar>
      </AppBar>

      {/* Conținut pe două coloane */}
      <Container sx={{ mt: 6 }}>
        <Stack
          direction={{ xs: "column", md: "row" }}
          spacing={4}
          alignItems="center"
          justifyContent="center"
        >
          {/* Text */}
          <Box sx={{ flex: 1, textAlign: { xs: "center", md: "left" } }}>
            <motion.div
              initial={{ opacity: 0, x: -30 }}
              animate={{ opacity: 1, x: 0 }}
              transition={{ duration: 0.8 }}
            >
              <Typography variant="h3" gutterBottom sx={{ fontWeight: "bold", color: "#004d40", fontSize: "45px" }}>
                Bine ai venit la Home IQ!
              </Typography>

              <Typography variant="h6" sx={{ mb: 2, color: "#00796b" }}>
                Inteligent. Confortabil. La un click distanță.
              </Typography>

              <Typography variant="body1" sx={{ mb: 4 }}>
                Monitorizează și controlează fiecare colț al casei tale, direct din aplicație.
              </Typography>

              <motion.div
                whileHover={{ scale: 1.05 }}
                whileTap={{ scale: 0.95 }}
                transition={{ type: "spring", stiffness: 300 }}
              >
                <Button
                  variant="contained"
                  size="large"
                  onClick={() => navigate("/login")}
                  sx={{
                    backgroundColor: "#00796b",
                    ":hover": { backgroundColor: "#004d40" },
                    px: 5,
                    py: 1.5,
                    fontWeight: "bold",
                  }}
                >
                  Conectare
                </Button>
              </motion.div>
            </motion.div>
          </Box>

          {/* Imagine */}
          <Box sx={{ flex: 1, textAlign: "center" }}>
            <motion.img
              src={SmartHomeImg}
              alt="Smart Home"
              style={{ maxWidth: "100%", height: "auto", borderRadius: "12px" }}
              initial={{ opacity: 0, x: 30 }}
              animate={{ opacity: 1, x: 0 }}
              transition={{ duration: 0.8 }}
            />
          </Box>
        </Stack>
      </Container>
    </Box>
  );
}



