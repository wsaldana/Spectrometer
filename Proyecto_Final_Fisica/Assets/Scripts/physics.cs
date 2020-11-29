using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using Random = Unity.Mathematics.Random;

public class physics : MonoBehaviour {
    public GameObject alpha;
    public GameObject electron;
    public GameObject tau;
    public GameObject neutron;
    public GameObject proton;
    public GameObject quark;
    public GameObject positron;
    public GameObject unknow;
    private GameObject clone1;
    private GameObject clone2;
    private GameObject clone3;
    private GameObject pref;

    //Dropdowns
    public GameObject pname1;
    public GameObject pname2;
    public GameObject pname3;

    //Campo para voltaje
    public GameObject voltajeField;

    private double efieldMagnitud;
    private double mfieldMagnitud;
    private double q;
    private double q2;
    private double q3;
    private double e;
    private double m;
    private double m2;
    private double m3;
    private double voltaje;
    private double B;
    private double B1;
    private double v_f;

    private double t1;
    private double t2;
    private double t3;


    //Componentes para el calculo de la masa de la particula customizable
    private double unknown_mass;
    private double unknownProton;
    private double unknownNeutron;
    private double unknown_energy;
    public GameObject protonNumberInput;
    public GameObject neutronNumberInput;

    Random rd = new Random(0x6E624EB7u);

    Vector2 vin = new Vector2(0, -1);

    void Start() {
        e = 1.60217646 * (Math.Pow(10, -19));
        B = 10.0e16;
        B1 = 10e-8;
    }

    void Update() {
        if (!clone1) {
            t1 = 0.0;
        }
        if (!clone2) {
            t2 = 0.0;
        }
        if (!clone3) {
            t3 = 0.0;
        }
    }

    private void FixedUpdate() {
        if (clone1) {
            if (clone1.transform.position.y > -1.0) {
                v_f = clone1.GetComponent<Rigidbody2D>().velocity.magnitude;
                double eForce = (efieldMagnitud * q);
                double mForce = -q * (B * v_f);
                clone1.GetComponent<Rigidbody2D>().AddForce(new Vector2((float)(eForce + mForce), 0), ForceMode2D.Impulse);
            } else {
                clone1.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                t1 += Time.deltaTime;
                double R = (m * v_f) / (q * B1);
                Debug.Log(R);
                double x_pos = R * Math.Cos(t1*v_f);
                double y_pos = -R * Math.Sin(t1 * v_f);
                clone1.transform.position = new Vector2((float)x_pos, (float)y_pos-1);
            }
        }

        if (clone2) {
            if (clone2.transform.position.y > -1.0) {
                v_f = clone2.GetComponent<Rigidbody2D>().velocity.magnitude;
                double eForce = (efieldMagnitud * q2);
                double mForce = -q2 * (B * v_f);
                clone2.GetComponent<Rigidbody2D>().AddForce(new Vector2((float)(eForce + mForce), 0), ForceMode2D.Impulse);
            } else {
                clone2.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                t2 += Time.deltaTime;
                double R = (m2 * v_f) / (q2 * B1);
                Debug.Log(R);
                double x_pos = R * Math.Cos(t2 * v_f);
                double y_pos = -R * Math.Sin(t2 * v_f);
                clone2.transform.position = new Vector2((float)x_pos, (float)y_pos - 1);
            }
        }

        if (clone3) {
            if (clone3.transform.position.y > -1.0) {
                v_f = clone3.GetComponent<Rigidbody2D>().velocity.magnitude;
                double eForce = (efieldMagnitud * q3);
                double mForce = -q3 * (B * v_f);
                clone3.GetComponent<Rigidbody2D>().AddForce(new Vector2((float)(eForce + mForce), 0), ForceMode2D.Impulse);
            } else {
                clone3.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                t3 += Time.deltaTime;
                double R = (m3 * v_f) / (q3 * B1);
                Debug.Log(R);
                double x_pos = R * Math.Cos(t3 * v_f);
                double y_pos = -R * Math.Sin(t3 * v_f);
                clone3.transform.position = new Vector2((float)x_pos, (float)y_pos - 1);
            }
        }
    }



    public void generate() {
        voltaje = double.Parse(voltajeField.GetComponent<Text>().text);
        efieldMagnitud = voltaje / (10e-17);

        string nameTxt1 = pname1.GetComponent<Text>().text;
        string nameTxt2 = pname2.GetComponent<Text>().text;
        string nameTxt3 = pname3.GetComponent<Text>().text;

        Vector2 vin = new Vector2(0, -1);
        int rand_num = rd.NextInt(10, 20);
        int rand_num2 = rd.NextInt(10, 20);
        int rand_num3 = rd.NextInt(10, 20);

        clone1 = instantiateParticle(nameTxt1, 1);
        clone1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -rand_num);
        Physics2D.IgnoreCollision(clone1.GetComponent<Collider2D>(), clone1.GetComponent<Collider2D>());

        if (!String.Equals(nameTxt2, "None")) {
            clone2 = instantiateParticle(nameTxt2, 2);
            clone2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -rand_num2);
            Physics2D.IgnoreCollision(clone1.GetComponent<Collider2D>(), clone2.GetComponent<Collider2D>());
        }

        if (!String.Equals(nameTxt3, "None")) {
            clone3 = instantiateParticle(nameTxt3, 3);
            clone3.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -rand_num3);
            Physics2D.IgnoreCollision(clone1.GetComponent<Collider2D>(), clone3.GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(clone2.GetComponent<Collider2D>(), clone3.GetComponent<Collider2D>());
        }
    }

    public double createMass() {
        if (protonNumberInput.GetComponent<Text>().text.Length != 0 || neutronNumberInput.GetComponent<Text>().text.Length != 0) {
            double.TryParse(protonNumberInput.GetComponent<Text>().text, out unknownProton);
            double.TryParse(neutronNumberInput.GetComponent<Text>().text, out unknownNeutron);
            unknown_mass = (unknownProton * 1.6727E-24) + (unknownNeutron * 1.6750E-24);
        }
        return unknown_mass;
    }

    public double calculateEnergy() {
        if (protonNumberInput.GetComponent<Text>().text.Length != 0 || neutronNumberInput.GetComponent<Text>().text.Length != 0) {
            double.TryParse(protonNumberInput.GetComponent<Text>().text, out unknownProton);
            unknown_energy = unknownProton * e;
        }
        return unknown_energy;
    }

    public GameObject instantiateParticle(String nameTxt, int num) {
        GameObject prefabParticle;
        double q0 = 0.0;
        double m0 = 0.0;
        switch (nameTxt) {
            case "Alpha":
                q0 = 2 * e;
                prefabParticle = alpha;
                m0 = 6.64466e-27;
                break;
            case "Electron":
                prefabParticle = electron;
                q0 = -e;
                m0 = 9.10938e-31;
                break;
            case "Tauon":
                prefabParticle = tau;
                q0 = -e;
                m0 = 3.167e-27;
                break;
            case "Neutron":
                prefabParticle = neutron;
                q0 = 0.0;
                m0 = 1.67493e-27;
                break;
            case "Proton":
                prefabParticle = proton;
                q0 = e;
                m0 = 1.67262e-27;
                break;
            case "Quark":
                prefabParticle = quark;
                q0 = -e / 3;
                m0 = 2.26398e-27;
                break;
            case "Positron":
                prefabParticle = positron;
                q0 = 2 * e / 3;
                m0 = 9.10938e-31;
                break;
            default:
                prefabParticle = unknow;
                q0 = calculateEnergy();
                m0 = createMass();
                break;
        }
        if (num == 1) {
            q = q0;
            m = m0;
        } else if (num == 2) {
            q2 = q0;
            m2 = m0;
        } else if (num == 3) {
            q3 = q0;
            m3 = m0;
        }
        return Instantiate(prefabParticle, new Vector2(3.27f, 4.47f), Quaternion.identity) as GameObject;
    }
}
