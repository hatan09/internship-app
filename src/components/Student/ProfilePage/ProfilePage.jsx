import React, { useState, useEffect } from 'react';
import {getStudent, getSkill} from '../../../api/Api'

function ProfilePage() {
    const [student, setStudent] = useState([]);

    async function loadData(){
        var id = localStorage.getItem('userId');
        var udata = await getStudent(id);
        var ids = udata.skillIds;
        udata['skills'] = [];
        var skills = [];
        ids.map(async id => {
            const skill = await getSkill(id);
            skills.push(skill);
            udata.skills = skills;
        });
        console.log(udata.skills);
        setStudent(udata);
    }

    function renderSwitch(){
        switch(student.stat) {
            case 0:
                return 'Pending';
            case 1:
                return 'Approved';
            case 2:
                return 'Denied';
            default:
                return 'Undified';
        }
    }

    useEffect(() => {
        loadData();
    }, []);

    return ( 
        <div className="profile-page">
            <div className="title">
                <span>{student.fullName} - Intern Profile </span>
            </div>
            <hr />
            <br />
            <div className="info">
                <table className="info-table">
                    <tbody>
                        <tr>
                            <td>
                                Fullname:
                            </td>
                            <td>
                                {student.fullName}
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Year of birth:
                            </td>
                            <td>
                                {student.birthdate}
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Email address:
                            </td>
                            <td>
                                {student.email}
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Skills:
                            </td>
                            <td>
                                <ol>{student.skills}
                                </ol>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Status:
                            </td>
                            <td>
                                <span>{renderSwitch()}</span>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    );
}

export default ProfilePage;
