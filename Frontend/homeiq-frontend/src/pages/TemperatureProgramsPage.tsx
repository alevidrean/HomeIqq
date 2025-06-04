import React, { useEffect, useState } from "react";
import {
  Box,
  Card,
  CardContent,
  Typography,
  TextField,
  Button,
  Divider,
  Stack,
} from "@mui/material";
import api from "../services/api";

type Interval = {
  id: number;
  start: string; // "HH:mm:ss" din backend
  end: string;
  temperature: number;
  programId: number;
};

type Program = {
  id: number;
  name: string;
  intervals: Interval[];
};

const pad = (n: number) => n.toString().padStart(2, "0");

// Convert "HH:mm:ss" <-> "HH:mm"
const toInputTime = (t: string) => t?.slice(0, 5) ?? "";
const toBackendTime = (t: string) => (t.length === 5 ? t + ":00" : t);

const TemperatureProgramsPage: React.FC = () => {
  const [programs, setPrograms] = useState<Program[]>([]);
  const [activeProgram, setActiveProgram] = useState<string>("");


  useEffect(() => {
  const fetchPrograms = async () => {
    try {
      const res = await api.get<Program[]>("/temperature-programs");
      setPrograms(res.data);
      const active = await api.get<{ name: string }>("/temperature-programs/active");
      setActiveProgram(active.data.name?.toLowerCase() ?? "");
    } catch {
      setPrograms([]);
      setActiveProgram("");
    }
  };
  fetchPrograms();
}, []);

  const handleIntervalChange = (
    programId: number,
    intervalId: number,
    field: "start" | "end" | "temperature",
    value: string
  ) => {
    setPrograms((prev) =>
      prev.map((prog) =>
        prog.id === programId
          ? {
              ...prog,
              intervals: prog.intervals.map((int) =>
                int.id === intervalId
                  ? {
                      ...int,
                      [field]:
                        field === "temperature"
                          ? parseInt(value)
                          : value, // păstrăm "HH:mm" în state
                    }
                  : int
              ),
            }
          : prog
      )
    );
  };

  const handleSave = async (program: Program) => {
    // Convertim intervalele la formatul așteptat de backend ("HH:mm:ss")
    const intervals = program.intervals.map((int) => ({
      ...int,
      start: toBackendTime(int.start),
      end: toBackendTime(int.end),
    }));
    await api.put(`/temperature-programs/${program.name}`, intervals);
    alert(`Programul '${program.name}' a fost salvat.`);
  };

  const handleSelectActive = async (name: string) => {
    await api.post(`/temperature-programs/select/${name}`);
    setActiveProgram(name);
      await api.post("/temperature/evaluate"); // trimite temperatura la ESP


  };

  return (
    <Box display="flex" gap={3} justifyContent="center" p={3} flexWrap="wrap">
      {programs.length === 0 && (
        <Typography>Nu există programe de temperatură în sistem.</Typography>
      )}
      {programs.map((program) => (
        <Card key={program.id} sx={{ width: 350, p: 2 }}>
          <CardContent>
            <Typography variant="h6" gutterBottom textTransform="capitalize">
              {program.name} {activeProgram === program.name && "(activ)"}
            </Typography>
            <Divider sx={{ mb: 2 }} />
            <Stack spacing={2}>
              {program.intervals.map((interval, idx) => (
                <Box key={interval.id}>
                  <Typography variant="subtitle2" sx={{ mb: 1 }}>
                    Interval {idx + 1}
                  </Typography>
                  <Box display="flex" gap={1}>
                    <TextField
                      label="Start"
                      type="time"
                      value={toInputTime(interval.start)}
                      onChange={(e) =>
                        handleIntervalChange(program.id, interval.id, "start", e.target.value)
                      }
                      size="small"
                      slotProps={{ inputLabel: { shrink: true } }}
                    />
                    <TextField
                      label="End"
                      type="time"
                      value={toInputTime(interval.end)}
                      onChange={(e) =>
                        handleIntervalChange(program.id, interval.id, "end", e.target.value)
                      }
                      size="small"
                      slotProps={{ inputLabel: { shrink: true } }}
                    />
                    <TextField
                      label="Temp (°C)"
                      type="number"
                      value={interval.temperature}
                      onChange={(e) =>
                        handleIntervalChange(program.id, interval.id, "temperature", e.target.value)
                      }
                      size="small"
                      slotProps={{
                        inputLabel: { shrink: true },
                        input: { inputProps: { min: 5, max: 35 } },
                      }}
                    />
                  </Box>
                </Box>
              ))}
            </Stack>
            <Button
              variant="contained"
              color="primary"
              onClick={() => handleSave(program)}
              sx={{ mt: 2, mr: 1 }}
              fullWidth
            >
              Salvează
            </Button>
            <Button
              variant={activeProgram === program.name ? "outlined" : "contained"}
              color="secondary"
              onClick={() => handleSelectActive(program.name)}
              sx={{ mt: 1 }}
              fullWidth
            >
              Setează ca activ
            </Button>
          </CardContent>
        </Card>
      ))}
    </Box>
  );
};

export default TemperatureProgramsPage;
