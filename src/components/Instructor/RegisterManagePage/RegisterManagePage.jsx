import React, { useState, useEffect } from 'react';
import {getAllReg, updateRegStat} from '../../../api/Api'
import RegisterItem from '../../RegisterItem.jsx';
import './RegisterManagePage.css'
import Modal from 'react-modal';
Modal.setAppElement('#root');


const customStyles = {
    content: {
      top: '50%',
      left: '50%',
      right: 'auto',
      bottom: 'auto',
      marginRight: '-50%',
      transform: 'translate(-50%, -50%)',
    },
};
const listHeaders = {id:0, fullName: 'Student Name', studentId: 'Student ID', birthdate: 'Year'};

function RegisterManagePage() {
    const [studentList, setStudentList] = useState([]);
    const [studentData, setStudentData] = useState([]);
    const [modalIsOpen, setIsOpen] = useState(false);

    async function loadData () {
        var grpId = localStorage.getItem('grpId');
        console.log(grpId);
        var students = await getAllReg(grpId);
        console.log(students);
        setStudentList(students);
    }

    useEffect(() => {
        loadData();
    }, []);

    async function approve(){
        const regId = studentData.id;
        const stat = 1;
        console.log(regId, stat);
        var result = await updateRegStat(regId, stat);
        closeModal();
    }

    async function deny(){
        const regId = studentData.id;
        const stat = 2;
        console.log(regId, stat);
        var result = await updateRegStat(regId, stat);
        closeModal();
    }

    function openModal(student) {
        setStudentData(student);
        setIsOpen(true);
    }
    
    function afterOpenModal() {
    }
    
    function closeModal() {
        setIsOpen(false);
    }
    
    return ( 
        <div className="register-manage-page">
            <div className="register-list">
                <div className="list-title">
                    <span>New Registrations</span>
                </div>
                <ul className="list">
                    <li id={listHeaders.id} style={{fontWeight: 'bold'}}> <RegisterItem item={listHeaders}></RegisterItem></li>
                    <hr />
                    {studentList.map(item => (
                        <li key={item.id}>
                            <RegisterItem item={item} onClick={openModal}/>
                        </li>
                    ))}
                </ul>
            </div>

            <Modal
                isOpen={modalIsOpen}
                onAfterOpen={afterOpenModal}
                onRequestClose={closeModal}
                style={customStyles}
                contentLabel="Example Modal"
            >
                <h1>Student Information</h1>
                <h3>{studentData.fullName}</h3>
                <p>{studentData.birthdate}</p>
                <p>{studentData.email}</p>
                <p>Credit acchieved: {studentData.credit}</p>
                <p>GPA: {studentData.gpa}</p>
                <p>Skills: </p>
                <ul>
                    {/* {studentData.skills.map(skill => (
                        <li style={{marginLeft: "20px"}} key={skill.id}> - {skill.name} ({skill.level})</li>
                    ))} */}
                </ul>
                

                <br />
                <button style={{padding: "3px", fontSize: "12pt", backgroundColor: "green"}} onClick={approve}>Accept</button>
                <button style={{marginLeft: "10px", padding: "3px", fontSize: "12pt", backgroundColor: "yellow"}} onClick={deny}>Reject</button>
                <br />
                <button style={{float: "right", padding: "3px", fontSize: "12pt", backgroundColor: "red"}} onClick={closeModal}>Close</button>
                
            </Modal>

            
        </div>
     );
}

export default RegisterManagePage;
