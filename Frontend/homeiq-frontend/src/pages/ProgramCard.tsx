// src/components/ProgramCard.tsx
import React, { useState } from "react";
import {
  Card,
  CardContent,
  Typography,
  TextField,
  Button,
  Box,
  Stack,
} from "@mui/material";

interface IntervalDto {
  id: number;
  start: string;
  end: string;
  temperature: number;
}

interface ProgramCardProps {
  program: { name: string; intervals: IntervalDto[] };
  onSave: (name: string, intervals: IntervalDto[]) => void;
  onActivate: (name: string) => void;
  isActive: boolean;
}

const ProgramCard: React.FC<ProgramCardProps> = ({
  program,
  onSave,
  onActivate,
  isActive,
}) => {
  const [intervals, setIntervals] = useState(program.intervals);

  const updateInterval = (index: number, field: keyof IntervalDto, value: any) => {
    const newIntervals = [...intervals];
    newIntervals[index] = { ...newIntervals[index], [field]: value };
    setIntervals(newIntervals);
  };

  return (
    <Card sx={{ width: 300, border: isActive ? "2px solid #00796b" : "1px solid #ccc" }}>
      <CardContent>
        <Typography variant="h6" textTransform="capitalize">
          {program.name} {isActive && "(activ)"}
        </Typography>
        <Stack spacing={1} mt={1}>
          {intervals.map((interval, i) => (
            <Box key={interval.id} display="flex" flexDirection="column">
              <TextField
                size="small"
                label="Start"
                type="time"
                value={interval.start}
                onChange={(e) => updateInterval(i, "start", e.target.value)}
              />
              <TextField
                size="small"
                label="End"
                type="time"
                value={interval.end}
                onChange={(e) => updateInterval(i, "end", e.target.value)}
              />
              <TextField
                size="small"
                label="Temperatura (°C)"
                type="number"
                inputProps={{ min: 5, max: 35 }}
                value={interval.temperature}
                onChange={(e) => updateInterval(i, "temperature", parseInt(e.target.value))}
              />
            </Box>
          ))}
        </Stack>
        <Box mt={2} display="flex" justifyContent="space-between">
          <Button variant="outlined" onClick={() => onSave(program.name, intervals)}>
            Salvează
          </Button>
          <Button
            variant="contained"
            color="primary"
            onClick={() => onActivate(program.name)}
            disabled={isActive}
          >
            Activează
          </Button>
        </Box>
      </CardContent>
    </Card>
  );
};

export default ProgramCard;
