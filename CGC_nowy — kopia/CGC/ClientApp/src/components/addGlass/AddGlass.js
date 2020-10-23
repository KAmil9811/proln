import React, { Component, useState } from "react";
import './AddGlass.css'

export class AddGlass extends Component {
    displayName = AddGlass.name;
    constructor(props) {
        super(props);
        this.state = {
            value: '',
            value2: '',

        }
    }

    handleAddGlass = (event) => {
        event.preventDefault();
        if (this.refs.height.value === "" || this.refs.width.value === "" || this.refs.length.value === "" || this.refs.owner.value === "" || this.refs.desk.value === "" || this.refs.color.value === "" || this.refs.type.value === "" || this.refs.amount.value === "" ) {
            alert("Uzupełnij dane")
        }
        else {
            const receiver = {
                glass: {
                    hight: this.refs.height.value,
                    width: this.refs.width.value,
                    length: this.refs.length.value,
                    owner: this.refs.owner.value,
                    desk: this.refs.desk.value,
                    color: this.refs.color.value,
                    type: this.refs.type.value,
                    count: this.refs.amount.value,
                },
                user: {
                    login: sessionStorage.getItem('login'),
                }
            }
            console.log(receiver)
            fetch(`api/Magazine/Add_Glass`, {
                method: "POST",
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(receiver
                ),
            })

                .then(res => res.json())
                .then(json => {
                    console.log(json)
                    return (json);
                    
                })
                .then(json => {
                    const glass2 = json[0].error_Messege
                    console.log(glass2)
                    if (glass2 == null) {
                        console.log("Dodano szkło")
                        this.props.history.push('/glasswarehouse')
                    }
                    else if (glass2 == "User_no_permission") {
                        alert("Nie posiadasz uprawnień aby dodać szkło");
                    }



                })
            
        }
    }

    cancelAddGlass = (event) => {
        this.props.history.push('/glasswarehouse')
    }

    render() {
        return (
            <div className="addGlass">
                <form>
                    <div className="form-group">
                        <h2>Dodawanie szkła</h2>
                        <label>Długość</label>
                        <input
                            type="number"
                            min="1"
                            name="Height"
                            className="form-control"
                            id="inputHeight"
                            placeholder="Podaj długość w milimetrach"
                            ref="length"
                        />
                    </div>
                    <div className="form-group">
                        <label>Szerokość</label>
                        <input
                            type="number"
                            min="1"
                            className="form-control"
                            id="inputWidth"
                            placeholder="Podaj szerokość w milimetrach"
                            ref="width"
                        />
                    </div>
                    <div className="form-group">
                        <label>Grubość</label>
                        <input
                            type="number"
                            min="1"
                            className="form-control"
                            id="inputLength"
                            placeholder="Podaj grubość w milimetrach"
                            ref="height"
                        />
                    </div>
                    <div className="form-group">
                        <label>Właściciel</label>
                        <input
                            type="text"
                            className="form-control"
                            id="inputOwner"
                            placeholder="Podaj właściciela"
                            ref="owner"
                        />
                    </div>
                    <div className="form-group">
                        <label>Kolor</label>
                        <input
                            type="text"
                            className="form-control"
                            id="inputColor"
                            placeholder="Podaj kolor"
                            ref="color"
                        />
                    </div>
                    <div className="form-group">
                        <label>Typ</label>
                        <input
                            type="text"
                            className="form-control"
                            id="inputType"
                            placeholder="Podaj typ szkła"
                            ref="type"
                        />
                    </div>
                    <div className="form-group">
                        <label>Miejsce</label>
                        <input
                            type="text"
                            className="form-control"
                            id="inputDesk"
                            placeholder="Podaj miejsce przechowania szkła"
                            ref="desk"
                        />
                    </div>
                    <div className="form-group">
                        <label>Ilosć</label>
                        <input
                            type="number"
                            min='1'
                            className="form-control"
                            id="inputDesk"
                            placeholder="Podaj liczbę"
                            ref="amount"
                        />
                    </div>


                    <div className="form-group">
                        
                        <button type="button" className="cancel_glass1" onClick={this.cancelAddGlass}>Anuluj</button>
                        <button type="button" className="add_glass1" onClick={this.handleAddGlass}>Dodaj</button>
                    </div>

                </form>
            </div>
        );
    }
}