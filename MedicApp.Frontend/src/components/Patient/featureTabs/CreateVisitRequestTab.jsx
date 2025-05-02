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
        <Typography variant="h5" fontWeight="500">
          Visit Requests
        </Typography>
        
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
      
      {/* Visit Requests Component */}
      <VisitRequestsComponent />

      {/* Create Visit Modal */}
      <CreateVisitModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
      />
    </Box>
  );
};

export default CreateVisitRequestTab;