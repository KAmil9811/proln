import React, { Component } from "react";
import './AddColor.css';
import Sidebar from '../Sidebar';


export class AddColor extends Component {
    displayName = AddColor.name;
    constructor(props) {
        super(props);
    }

    handleAddColor = (event) => {
        event.preventDefault();
        const receiver = {
            color: this.refs.color.value,
            user: {
                login: sessionStorage.getItem('login')
            }
        }

        fetch(`api/Magazine/Add_Color_Admin`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Content-Type': 'application/json'
            }
        })

            .then(res => res.json())
            .then(json => {
                console.log(json)
                return (json);
            })
            .then(json => {
                alert("Dodano kolor do bazy danych")
                this.props.history.push('/glassatibutes')
            })
    }

    cancelAddColor = (event) => {
        this.props.history.push('/glassatibutes')
    }

    render() {
        return (
            <div className="AddColor">
                   <Sidebar/>
                    <div className="addColor">
                        <form>
                            <div className="form-group">
                                <h2>Dodaj kolor szkła:</h2>
                                <input
                                    type="text"
                                    className="form-control"
                                    id="inputColor"
                                    placeholder="Wprowadź kolor szkła"
                                    ref="color"
                                />
                            </div>
                            <div className="form-group">
                                <button type="button" className="cancel_add_gla_c" onClick={this.cancelAddColor}>Anuluj</button>
                                <button type="button" className="adding_glass_c" onClick={this.handleAddColor}>Dodaj</button>

                            </div>

                        </form>
                </div>
            </div>
        );
    }
}