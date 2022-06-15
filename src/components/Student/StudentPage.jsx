import React, { useState, useEffect } from 'react';
import {getStudent} from '../../api/Api'
import { Outlet } from 'react-router-dom';
import PageBanner from '../PageBanner';
import SideBar from '../SideBar';


function StudentPage() {
    const [title, setTitle] = useState("Student Page");

    async function loadData () {
        var id = localStorage.getItem('userId');
        var student = await getStudent(id);
        var depId = student.departmentId;
        localStorage.setItem('depId', depId);
    }

    useEffect(() => {
        loadData();
    }, []);

    const NavLinks = [
        {   
            id: 1,
            title: 'Jobs',
            path: "/student/jobs",
            func: () => setTitle("Jobs")
        },
        {   
            id: 2,
            title: 'Profile',
            path: "/student/profile",
            func: () => setTitle("Profile")
        }
    ]

    return (
        <div className="student-page">
            <PageBanner title={title}/>
            <SideBar NavLinks={NavLinks}/>
            <div className="right">
            <Outlet/>
            </div>
            
        </div>
    );
}

export default StudentPage;
