import React, { Component } from 'react';
import './glassColorEdit.css';
import Sidebar from '../Sidebar';

export class GlassColorEdit extends Component {
    displayName = GlassColorEdit.name;
    constructor(props) {
        super(props);
        this.state = {
            value: '',
        }
    }
    changeColor = (event) => {
        event.preventDefault();
        const receiver = {
            user: {
                login: sessionStorage.getItem('login')
            },
            new_color: this.refs.color.value,
            old_color: sessionStorage.getItem('color')
        }
        fetch(`api/Magazine/Change_Color_Admin`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Authorization': 'Bearer ' + sessionStorage.getItem('token'),
                'Content-Type': 'application/json'
            }
            })
                .then(res => res.json())
                .then(json => {
                   
                    return (json);
                })
            .then(json => {
                if (json[0] === 'New_Color_already_exist') {
                    alert("New color already exist")
                }
                else {
                    alert("Color has been edited")
                    this.props.history.push('/glassatibutes')
                    sessionStorage.removeItem('color')
                }
                })
    }

    return = (event) => {
        event.preventDefault();
        sessionStorage.removeItem('color')
        this.props.history.push('/glassatibutes')
    }
    
    render() {
        if ((sessionStorage.getItem('valid') === '') && (sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true')) {
            return (
                <div className="HomePage">
                    <h1>Log in to have access!</h1>
                    <button type="submit" className="success_login" onClick={this.goback} >Log in</button>
                </div>
            );
        }
        else if (sessionStorage.getItem('superAdmin') === 'true' || sessionStorage.getItem('manager') === 'true' || sessionStorage.getItem('admin') === 'true') {
            return (

                <div className="editColor">
                    <Sidebar />
                    <div className="title">
                        <h1 className="titletext">Edit glass color</h1>
                    </div>
                    <div className="EditColor">
                        <form>
                            <div className="form-group">

                                <input
                                    type="text"
                                    name="color"
                                    className="form-control"
                                    id="inputColor"
                                    placeholder="Podaj kolor"
                                    ref="color"
                                    defaultValue={sessionStorage.getItem('color')}
                                />
                            </div>

                            <button type="button" className="danger_color_edit" onClick={this.return}>Cancel</button>
                            <button type="button" className="success_color_edit" onClick={this.changeColor}>Edit</button>
                        </form>
                    </div>
                </div>

            )
        }
        else {
            return (
                <div className="HomePage">
                    <h1>Check if you have perrmission to this panel</h1>
                    <button type="submit" className="success_login" onClick={this.goback2} >Back to home page</button>
                </div>
            );
        }
    }
}