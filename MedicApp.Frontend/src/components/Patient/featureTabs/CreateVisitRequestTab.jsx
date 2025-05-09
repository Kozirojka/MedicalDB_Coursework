import { useState } from "react";
import CreateVisitModal from "../CreateVisitModal";
import VisitRequestsComponent from "../VisitRequestsComponent";
import { Button, Box, Typography, Divider } from "@mui/material";
import AddIcon from '@mui/icons-material/Add';

const CreateVisitRequestTab = () => {
  const [isModalOpen, setIsModalOpen] = useState(false);

  return (
    <Box sx={{ p: 2 }}>
      <Box sx={{ 
        display: 'flex', 
        justifyContent: 'space-between', 
        alignItems: 'center',
        mb: 3
      }}>
        
        <Button
          variant="contained"
          color="primary"
          startIcon={<AddIcon />}
          onClick={() => setIsModalOpen(true)}
          sx={{ fontWeight: 'bold' }}
        >
          Create visit request
        </Button>
      </Box>
      
      <Divider sx={{ mb: 3 }} />
      
      <VisitRequestsComponent />

      <CreateVisitModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
      />
    </Box>
  );
};

export default CreateVisitRequestTab;