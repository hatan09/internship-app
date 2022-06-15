import React, { useState, useEffect } from 'react';
import {getAllStudent} from '../../../api/Api'
import './StudentManagePage.css'
import Modal from 'react-modal';
import StudentItem from '../../StudentItem';
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
const listHeaders = {id:0, fullName: 'Student Name', birthdate: 'Year', isAccepted: 'Found Job?'};

function StudentManagePage() {
    const [studentList, setStudentList] = useState([]);
    const [studentData, setStudentData] = useState([]);
    const [modalIsOpen, setIsOpen] = useState(false);

    async function loadData () {
        var grpId = localStorage.getItem('grpId');
        console.log(grpId);
        var students = await getAllStudent(grpId);
        console.log(students);
        setStudentList(students);
    }

    useEffect(() => {
        loadData();
    }, []);

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
        <div className="student-manage-page">
            <div className="student-list">
                <div className="list-title">
                    <span>Students</span>
                </div>
                <ul className="list">
                    <li id={listHeaders.id} style={{fontWeight: 'bold'}}> <StudentItem item={listHeaders}></StudentItem></li>
                    <hr />
                    {studentList.map(item => (
                        <li key={item.id}>
                            <StudentItem item={item} onClick={openModal}/>
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
                <p>GPA: {studentData.gpa}</p>
                

                <br />
                <button style={{padding: "3px", fontSize: "12pt"}}>Notify</button>
                <button style={{marginLeft: "10px", padding: "3px", fontSize: "12pt"}}>Copy Email</button>
                <br />
                <button style={{float: "right", padding: "3px", fontSize: "12pt", backgroundColor: "red"}} onClick={closeModal}>Close</button>
                
            </Modal>
        </div>
    );
}

export default StudentManagePage;