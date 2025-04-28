import React, { useState } from "react";
import { BASE_API } from "../../constants/BASE_API";
import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  FormControl,
  FormGroup,
  TextField,
  Typography,
} from "@mui/material";

async function createVisitRequest(visitData) {
  try {

    const token = localStorage.getItem("accessToken");
    console.log("Дані, які надсилаються:", visitData);
    console.log(token);

    console.log(visitData);
    const response = await fetch(`${BASE_API}/appointment`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify(visitData),
    });

    if (!response.ok) {
      throw new Error("Помилка при створенні запиту");
    }

    return await response.json();
  } catch (error) {
    console.error("Помилка:", error);
    throw error;
  }
}

export default function CreateVisitModal({ isOpen, onClose }) {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);

  const [visitData, setVisitData] = useState({
    description: "",
  });

  const handleInputChange = (e) => {
    const { name, value, type, checked } = e.target;
    setVisitData((prev) => ({
      ...prev,
      [name]: type === "checkbox" ? checked : value,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsLoading(true);
    setError(null);

    try {
      await createVisitRequest(visitData);
      onClose();
      alert("Запит створено успішно!");
    } catch (error) {
      setError("Не вдалося створити запит. Спробуйте пізніше.");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Dialog
      open={isOpen}
      onClose={onClose}
      aria-labelledby="form-dialog-title"
      fullWidth
      maxWidth="sm"
    >
      <DialogTitle id="form-dialog-title">Create Visit Request</DialogTitle>
      <DialogContent>
        {error && (
          <Typography color="error" gutterBottom>
            {error}
          </Typography>
        )}
        <FormGroup>

          
          <FormControl fullWidth margin="normal">
            <TextField
              label="Description"
              name="description"
              value={visitData.description}
              onChange={handleInputChange}
              required
              multiline
              rows={4}
            />
          </FormControl>
        </FormGroup>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose} color="secondary">
          Cancel
        </Button>
        <Button onClick={handleSubmit} color="primary" disabled={isLoading}>
          {isLoading ? "Creating..." : "Create Request"}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
